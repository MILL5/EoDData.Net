using System.Net.Http;
using AutoMapper;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public interface IEoDDataDependencies
    {
        EoDDataSettings Settings { get; set; }
        IHttpClientFactory HttpClientFactory { get; set; }
        IMapper Mapper { get; set; }

    }

    internal class EoDDataDependencies : IEoDDataDependencies
    {
        public EoDDataSettings Settings { get; set; }
        public IHttpClientFactory HttpClientFactory { get; set; }
        public IMapper Mapper { get; set; }

        public EoDDataDependencies(EoDDataSettings settings, IHttpClientFactory clientFactory, IMapper mapper)
        {
            CheckIsNotNull(nameof(settings), settings);
            CheckIsNotNull(nameof(clientFactory), clientFactory);

            Settings = settings;
            HttpClientFactory = clientFactory;
            Mapper = mapper;
        }
    }
}