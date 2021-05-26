using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string EXCHANGE_GET_ENDPOINT = "/ExchangeGet?Exchange=";

        public async Task<EXCHANGE> ExchangeGetAsync(string exchange)
        {
            var requestUrl = $"{ _settings.ApiBaseUrl }{ EXCHANGE_GET_ENDPOINT }{ exchange }";

            var exchanges = await Get<RESPONSE>(requestUrl);

            return exchanges;
        }
    }
}