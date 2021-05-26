namespace EoDData.Net
{
    public class EoDDataSettings
    {
        private const string EODDATA_API_BASE_URL = "http://ws.eoddata.com/data.asmx";

        private const string EODDATA_HTTPCLIENT_NAME = "EoDDataHttpClient";

        public string LoginToken { get; set; }

        public string ApiBaseUrl
        {   
            get
            {
                return EODDATA_API_BASE_URL;
            } 
        }

        public string HttpClientName
        {
            get
            {
                return EODDATA_HTTPCLIENT_NAME;
            }
        }
    }
}