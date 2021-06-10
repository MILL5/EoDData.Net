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
        private const string INTERNAL_SERVER_ERROR = "internal server error";
        private const string INVALID_TOKEN = "invalid token";
        private const string INVALID_USR_PASS = "invalid username or password";
        private const string SUCCESS_MESSAGE = "success";

        private readonly EoDDataSettings _settings;
        private readonly IHttpClientFactory _httpClient;
        private readonly IMapper _mapper;

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
                // TODO: change for a retry operation. Polly.
                if (ex.Message.ToLowerInvariant() == INTERNAL_SERVER_ERROR)
                {
                    response = await GetWithLoginCheck<T>(requestUrl).ConfigureAwait(false);
                }
                else if (ex.Message.ToLowerInvariant() == INVALID_TOKEN)
                {
                    _settings.ApiLoginToken = string.Empty;
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
            if (string.IsNullOrEmpty(_settings.ApiLoginToken))
            {
                await Login().ConfigureAwait(false);
            }

            var eodDataResponse = await GetDeserializedResponse<T>(requestUrl).ConfigureAwait(false);
            var message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();

            if (message.ToLowerInvariant() != SUCCESS_MESSAGE)
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

            if (response.Message.ToLowerInvariant() == INVALID_USR_PASS)
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