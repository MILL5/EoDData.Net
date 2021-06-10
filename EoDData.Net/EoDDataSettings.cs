namespace EoDData.Net
{
    public class EoDDataSettings
    {
        private const string EODDATA_HTTPCLIENT_NAME = "EoDDataHttpClient";

        public string ApiUsername { get; }

        public string ApiPassword { get; }

        internal string ApiLoginToken { get; set; }

        public string ApiBaseUrl { get; }

        public int TimeOutInSeconds { get; set; } = 180;

        internal string HttpClientName { get; } = EODDATA_HTTPCLIENT_NAME;

        public EoDDataSettings(string apiBaseUrl, string apiUsername, string apiPassword)
        {
            ApiBaseUrl = apiBaseUrl;
            ApiUsername = apiUsername;
            ApiPassword = apiPassword;
        }
    }
}