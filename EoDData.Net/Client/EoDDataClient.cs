using AutoMapper;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string INVALID_TOKEN = "Invalid Token";
        private const string INVALID_USR_PASS = "Invalid Username or Password";
        private const string SUCCESS_MESSAGE = "Success";

        private readonly EoDDataSettings _settings;
        private readonly IHttpClientFactory _httpClient;
        private readonly IMapper _mapper;

        private static readonly object lockObj = new();

        public EoDDataClient(IEoDDataDependencies dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);
            _httpClient = dependencies.HttpClientFactory;
            _settings = dependencies.Settings;
            _mapper = dependencies.Mapper;
        }

        private async Task<T> Get<T>(string requestUrl)
        {
            return await GetHandleEoDInvalidTokenHttpError<T>(requestUrl).ConfigureAwait(false);
        }

        // On some routes EoD Data sends a 500 for a invalid token.
        private async Task<T> GetHandleEoDInvalidTokenHttpError<T>(string requestUrl)
        {
            T response;
            try
            {
                response = await GetWithLoginCheck<T>(requestUrl).ConfigureAwait(false);
            }
            catch (EoDDataHttpException ex)
            {
                if (string.Equals(ex.Message, INVALID_TOKEN, StringComparison.OrdinalIgnoreCase))
                {
                    lock (lockObj)
                    {
                        _settings.ApiLoginToken = string.Empty;
                    }

                    response = await GetWithLoginCheck<T>(requestUrl).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }

            return response;
        }

        private async Task<T> GetWithLoginCheck<T>(string requestUrl)
        {
            lock (lockObj)
            {
                if (string.IsNullOrEmpty(_settings.ApiLoginToken))
                {
                    Login().GetAwaiter().GetResult();
                }
            }

            var eodDataResponse = await GetDeserializedResponse<T>(requestUrl).ConfigureAwait(false);
            var message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();

            if (!string.Equals(message, SUCCESS_MESSAGE, StringComparison.OrdinalIgnoreCase))
            {
                throw new EoDDataHttpException($"{ message }");
            }

            return eodDataResponse;
        }

        private async Task<T> GetDeserializedResponse<T>(string requestUrl)
        {
            using var client = _httpClient.CreateClient(_settings.HttpClientName);
            requestUrl = $"{ requestUrl }{ (requestUrl.Contains("?") ? "&" : "?") }Token={ _settings.ApiLoginToken }";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await client.SendAsync(request).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new EoDDataHttpException(response.ReasonPhrase);
            }

            var deserializedResponse = await DeserializeResponse<T>(response).ConfigureAwait(false);

            return deserializedResponse;
        }

        private async Task Login()
        {
            var requestUrl = $"Login?Username={ _settings.ApiUsername }&Password={ _settings.ApiPassword }";

            var response = await GetDeserializedResponse<LoginResponse>(requestUrl).ConfigureAwait(false);

            if (string.Equals(response.Message, INVALID_USR_PASS, StringComparison.OrdinalIgnoreCase))
            {
                throw new EoDDataHttpException(INVALID_USR_PASS);
            }

            _settings.ApiLoginToken = response.Token;
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var xmlStr = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            try
            {
                var serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(xmlStr);
            }
            catch (Exception ex)
            {
                throw new EoDDataHttpException(ex.Message);
            }
        }
    }
}