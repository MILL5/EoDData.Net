using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private readonly IEoDDataDependencies _dependencies;
        public readonly EoDDataSettings _settings;

        public EoDDataClient(IEoDDataDependencies dependencies)
        {
            CheckIsNotNull(nameof(dependencies), dependencies);
            _dependencies = dependencies;
            _settings = dependencies.Settings;
        }

        private async Task<string> Get(string requestUrl)
        {
            using var client = _dependencies.HttpClientFactory.CreateClient(_settings.HttpClientName);

            requestUrl = $"{ requestUrl }";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new EoDDataHttpException(response.ReasonPhrase);
            }

            return await response.Content.ReadAsStringAsync();
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