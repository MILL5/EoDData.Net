namespace EoDData.Net
{
    public class EoDDataSettings
    {
        private const string EODDATA_API_BASE_URL = "";

        private const string EODDATA_HTTPCLIENT_NAME = "EoDDataHttpClient";

        public string ApiBaseUrl 
        {   
            get
            {
                return EODDATA_API_BASE_URL;
            } 
        }

        public string ApiKey { get; set; }

        public bool UsePremiumOptions { get; set; }

        public string HttpClientName
        {
            get
            {
                return EODDATA_HTTPCLIENT_NAME;
            }
        }
    }
}