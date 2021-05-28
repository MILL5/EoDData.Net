using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string INVALID_TOKEN = "Invalid Token";
        private const string NOT_LOGGED_IN = "Not logged in";
        private const string INVALID_USR_PASS = "Invalid Username or Password";
        private const string SUCCESS_MESSAGE = "Success";

        private readonly IEoDDataDependencies _dependencies;
        public EoDDataSettings _settings;

        public EoDDataClient(IEoDDataDependencies dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);
            _dependencies = dependencies;
            _settings = dependencies.Settings;
        }

        private async Task<T> Get<T>(string requestUrl)
        {
            var eodDataResponse = await GetRequest<T>(requestUrl);

            var message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();
            if (message == INVALID_TOKEN || message == NOT_LOGGED_IN)
            {
                await Login();
                eodDataResponse = await GetRequest<T>(requestUrl);
                message = eodDataResponse.GetType().GetProperty(nameof(BaseResponse.Message)).GetValue(eodDataResponse).ToString();
            }

            if (message != SUCCESS_MESSAGE)
            {
                throw new EoDDataHttpException($"EoD Data responded with the following error message: { message }");
            }

            return eodDataResponse;
        }

        private async Task<T> GetRequest<T>(string requestUrl)
        {
            using var client = _dependencies.HttpClientFactory.CreateClient(_settings.HttpClientName);

            requestUrl = $"{ requestUrl }{ (requestUrl.Contains("?") ? "&" : "?") }Token={ _settings.ApiLoginToken }";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new EoDDataHttpException(response.ReasonPhrase);
            }

            var desResponse = await DeserializeResponse<T>(response);

            return desResponse;
        }

        private async Task Login()
        {
            var requestUrl = $"Login?Username={ _settings.ApiUsername }&Password={ _settings.ApiPassword }";
            
            var response = await GetRequest<LoginResponse>(requestUrl);

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
            catch (Exception)
            {
                throw new EoDDataHttpException("There was an error deserializing the EoD Response xml content.");
            }
        }

        private string GetQueryParameterString(Dictionary<string, string> queryParams)
        {
            var sb = new StringBuilder();

            foreach (var qp in queryParams)
            {
                if (qp.Value != null)
                {
                    sb.Append($"&{ qp.Key }={ qp.Value }");
                }
            }

            if (sb.Length == 0)
            {
                return string.Empty;
            }

            sb.Remove(0, 1);
            sb.Insert(0, "?");

            return sb.ToString();
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