using AutoMapper;
using System.Net.Http;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public interface IEoDDataDependencies
    {
        EoDDataSettings Settings { get; }
        IHttpClientFactory HttpClientFactory { get; }
        IMapper Mapper { get; }
    }

    public class EoDDataDependencies : IEoDDataDependencies
    {
        public IHttpClientFactory HttpClientFactory { get; internal set; }
        public IMapper Mapper { get; internal set; }
        public EoDDataSettings Settings { get; internal set; }

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