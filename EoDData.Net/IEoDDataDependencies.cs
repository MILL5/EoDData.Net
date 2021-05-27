using System.Net.Http;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public interface IEoDDataDependencies
    {
        EoDDataSettings Settings { get; set; }
        IHttpClientFactory HttpClientFactory { get; set; }
    }

    internal class EoDDataDependencies : IEoDDataDependencies
    {
        public EoDDataSettings Settings { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }

        public EoDDataDependencies(EoDDataSettings settings, IHttpClientFactory clientFactory)
        {
            CheckIsNotNull(nameof(settings), settings);
            CheckIsNotNull(nameof(clientFactory), clientFactory);

            Settings = settings;
            HttpClientFactory = clientFactory;
        }
    }
}