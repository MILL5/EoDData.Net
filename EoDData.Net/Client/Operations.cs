using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string EXCHANGE_GET_ENDPOINT = "ExchangeGet?Exchange=";

        public async Task<Exchange> ExchangeGetAsync(string exchange)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = $"{ EXCHANGE_GET_ENDPOINT }{ exchange }";

            var exchangeResponse = await Get<ExchangeResponse>(requestUrl);

            return exchangeResponse.Exchange;
        }
    }
}