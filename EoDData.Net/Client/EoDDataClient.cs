using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AutoMapper;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string INTERNAL_SERVER_ERROR = "Internal Server Error";
        private const string INVALID_TOKEN = "Invalid Token";
        private const string NOT_LOGGED_IN = "Not logged in";
        private const string INVALID_USR_PASS = "Invalid Username or Password";
        private const string SUCCESS_MESSAGE = "Success";

        private readonly IEoDDataDependencies _dependencies;
        public EoDDataSettings _settings;
        public readonly IMapper _mapper;

        public EoDDataClient(IEoDDataDependencies dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);
            _dependencies = dependencies;
            _settings = dependencies.Settings;
            _mapper = dependencies.Mapper;
        }

        private async Task<T> Get<T>(string requestUrl)
        {
            return await GetHandleEoDInvalidTokenHttpError<T>(requestUrl);
        }

        // On some routes EoD Data sends a 500 for a invalid token.
        private async Task<T> GetHandleEoDInvalidTokenHttpError<T>(string requestUrl)
        {
            T response;
            try
            {
                response = await GetWithLoginCheck<T>(requestUrl);
            }
            catch (EoDDataHttpException ex)
            {
                if (ex.Message == INTERNAL_SERVER_ERROR)
                {
                    _settings.ApiLoginToken = null;
                    response = await GetWithLoginCheck<T>(requestUrl);
                }
                else
                {
                    throw ex;
                }
            }

            return response;
        }

        private async Task<T> GetWithLoginCheck<T>(string requestUrl)
        {
            var eodDataResponse = await GetDeserializedResponse<T>(requestUrl);

            var message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();
            if (message == INVALID_TOKEN || message == NOT_LOGGED_IN)
            {
                await Login();
                eodDataResponse = await GetDeserializedResponse<T>(requestUrl);
                message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();
            }

            if (message != SUCCESS_MESSAGE)
            {
                throw new EoDDataHttpException($"EoD Data responded with the following error message: { message }");
            }

            return eodDataResponse;
        }

        private async Task<T> GetDeserializedResponse<T>(string requestUrl)
        {
            using var client = _dependencies.HttpClientFactory.CreateClient(_settings.HttpClientName);

            requestUrl = $"{ requestUrl }{ (requestUrl.Contains("?") ? "&" : "?") }Token={ _settings.ApiLoginToken }";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new EoDDataHttpException(response.ReasonPhrase);
            }

            var deserializedResponse = await DeserializeResponse<T>(response);

            return deserializedResponse;
        }

        private async Task Login()
        {
            var requestUrl = $"Login?Username={ _settings.ApiUsername }&Password={ _settings.ApiPassword }";
            
            var response = await GetDeserializedResponse<LoginResponse>(requestUrl);

            if(response.Message == INVALID_USR_PASS)
            {
                throw new EoDDataHttpException("Could not login to the EoD Data Account.");
            }

            _settings.ApiLoginToken = response.Token;
        }

        private async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var xmlStr = await response.Content.ReadAsStringAsync();
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(xmlStr);

                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new EoDDataHttpException(ex.Message);
            }
        }

        private string FormatDateString(string inputDateString)
        {
            if (inputDateString == null)
                return null;

            var dateIsParsed = DateTime.TryParse(inputDateString, out DateTime outTime);

            if (!dateIsParsed)
            {
                try
                {
                    outTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(inputDateString)).UtcDateTime;
                }
                catch
                {
                    throw new Exception("Invalid date input.");
                }
            }

            return outTime.ToString("yyyy-MM-dd");
        }
    }
}