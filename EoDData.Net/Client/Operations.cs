using System.Collections.Generic;
using System.Threading.Tasks;
using static Pineapple.Common.Preconditions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        private const string EXCHANGE_GET_ENDPOINT = "ExchangeGet?Exchange={0}";

        private const string EXCHANGE_LIST_ENDPOINT = "ExchangeList";

        private const string SYMBOL_GET_ENDPOINT = "SymbolGet?Exchange={0}&Symbol={1}";

        private const string SYMBOL_LIST_ENDPOINT = "SymbolList?Exchange={0}";

        private const string SYMBOL_HISTORY_ENDPOINT = "SymbolHistory?Exchange={0}&Symbol={1}&StartDate={2}";

        private const string QUOTE_GET_ENDPOINT = "QuoteGet?Exchange={0}&Symbol={1}";

        private const string QUOTE_LIST_ENDPOINT = "QuoteListByDate?Exchange={0}&QuoteDate={1}";

        public async Task<Exchange> ExchangeGetAsync(string exchange)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = string.Format(EXCHANGE_GET_ENDPOINT, exchange);

            var exchangeGetResponse = await Get<ExchangeGetResponse>(requestUrl);

            return exchangeGetResponse.Exchange;
        }

        public async Task<List<Exchange>> ExchangeListAsync()
        {
            var exchangeListResponse = await Get<ExchangeListResponse>(EXCHANGE_LIST_ENDPOINT);

            return exchangeListResponse.Exchanges.ExchangeList;
        }

        public async Task<Symbol> SymbolGetAsync(string exchange, string symbol)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);

            var requestUrl = string.Format(SYMBOL_GET_ENDPOINT, exchange, symbol);

            var symbolGetResponse = await Get<SymbolGetResponse>(requestUrl);

            return symbolGetResponse.Symbol;
        }

        public async Task<List<Symbol>> SymbolListAsync(string exchange)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = string.Format(SYMBOL_LIST_ENDPOINT, exchange);

            var symbolListResponse = await Get<SymbolListResponse>(requestUrl);

            return symbolListResponse.Symbols.SymbolList;
        }

        public async Task<List<Quote>> SymbolHistoryAsync(string exchange, string symbol, string startDate)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);
            CheckIsNotNullOrWhitespace(nameof(startDate), startDate);

            var requestUrl = string.Format(SYMBOL_HISTORY_ENDPOINT, exchange, symbol, startDate);

            var symbolListResponse = await Get<QuoteListResponse>(requestUrl);

            return symbolListResponse.Quotes.QuoteList;
        }

        public async Task<Quote> QuoteGetAsync(string exchange, string symbol)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);

            var requestUrl = string.Format(QUOTE_GET_ENDPOINT, exchange, symbol);

            var quoteGetResponse = await Get<QuoteGetResponse>(requestUrl);

            return quoteGetResponse.Quote;
        }

        public async Task<List<Quote>> QuoteListAsync(string exchange, string date)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(date), date);

            var requestUrl = string.Format(QUOTE_LIST_ENDPOINT, exchange, date);

            var quoteListResponse = await Get<QuoteListResponse>(requestUrl);

            return quoteListResponse.Quotes.QuoteList;
        }
    }
}