using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public static class RegisterAssembly
    {
        const string EODDATA_USERNAME = "EoDDataUsername";

        const string EODDATA_PASSWORD = "EoDDataPassword";

        public static void AddServices(IServiceCollection services, IConfiguration config)
        {
            CheckIsNotNull(nameof(services), services);
            CheckIsNotNull(nameof(config), config);

            CheckIsNotNull(EODDATA_USERNAME, config[EODDATA_USERNAME]);
            CheckIsNotNull(EODDATA_PASSWORD, config[EODDATA_PASSWORD]);

            var settings = new EoDDataSettings()
            {
                ApiUsername = config[EODDATA_USERNAME],
                ApiPassword = config[EODDATA_PASSWORD]
            };

            services.AddSingleton(settings);
            services.AddTransient<IEoDDataDependencies, EoDDataDependencies>();
            services.AddTransient<IEoDDataClient, EoDDataClient>();

            AddHttpClient(services, settings);
        }

        private static void AddHttpClient(IServiceCollection services, EoDDataSettings settings)
        {
            services.AddTransient<BrotliCompressionHandler>();
            services.AddHttpClient(settings.HttpClientName, client =>
            {
                client.BaseAddress = new Uri(settings.ApiBaseUrl);

            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip

            }).AddHttpMessageHandler<BrotliCompressionHandler>();
        }
    }
}