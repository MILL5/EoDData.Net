using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public static class RegisterAssembly
    {
        const string EODDATA_API_KEY_NAME = "EoDDataApiKey";

        const string USE_PREM_OPTIONS_NAME = "UsePremiumOptions";

        public static void AddServices(IServiceCollection services, IConfiguration config)
        {
            CheckIsNotNull(nameof(services), services);
            CheckIsNotNull(nameof(config), config);

            CheckIsNotNull(EODDATA_API_KEY_NAME, config[EODDATA_API_KEY_NAME]);
            
            var settings = new EoDDataSettings
            {
                ApiKey = config[EODDATA_API_KEY_NAME],
                UsePremiumOptions = config[USE_PREM_OPTIONS_NAME] != null && bool.Parse(config[USE_PREM_OPTIONS_NAME])
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
                client.DefaultRequestHeaders.Add("Accept", "application/json");

            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AllowAutoRedirect = false,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip

            }).AddHttpMessageHandler<BrotliCompressionHandler>();
        }
    }
}