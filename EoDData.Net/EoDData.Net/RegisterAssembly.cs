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

        public static async Task AddServices(IServiceCollection services, IConfiguration config)
        {
            CheckIsNotNull(nameof(services), services);
            CheckIsNotNull(nameof(config), config);

            CheckIsNotNull(EODDATA_USERNAME, config[EODDATA_USERNAME]);
            CheckIsNotNull(EODDATA_PASSWORD, config[EODDATA_PASSWORD]);

            var settings = new EoDDataSettings();

            await SetEoDDataLoginToken(settings, config);

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

        private static async Task SetEoDDataLoginToken(EoDDataSettings settings, IConfiguration config)
        {
            var httpClient = new HttpClient();

            var requestUrl = $"{ settings.ApiBaseUrl }/Login?Username={ config[EODDATA_USERNAME] }&Password={ config[EODDATA_PASSWORD] }";

            // TODO : Add error handling
            var response = await httpClient.GetAsync(requestUrl);

            var contentStream = await response.Content.ReadAsStreamAsync();

            var serializer = new XmlSerializer(typeof(LoginResponse));

            var loginResponseObj = (LoginResponse)serializer.Deserialize(contentStream);

            settings.LoginToken = loginResponseObj.Token;
        }
    }
}