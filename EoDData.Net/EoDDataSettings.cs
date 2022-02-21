namespace EoDData.Net
{
    public class EoDDataSettings
    {
        private const string EODDATA_HTTPCLIENT_NAME = "EoDDataHttpClient";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Need to use const to build Urls")]
        private const string EODDATA_BASE_Url = "http://ws.eoddata.com/data.asmx/";

        public string ApiUsername { get; }

        public string ApiPassword { get; }

        internal string ApiLoginToken { get; set; }

        public string ApiBaseUrl { get; }

        public int TimeOutInSeconds { get; set; } = 180;

        internal string HttpClientName { get; } = EODDATA_HTTPCLIENT_NAME;

        public EoDDataSettings(string apiUsername, string apiPassword)
        {
            ApiBaseUrl = EODDATA_BASE_Url;
            ApiUsername = apiUsername;
            ApiPassword = apiPassword;
        }
    }
}
