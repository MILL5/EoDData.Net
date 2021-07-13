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

        private const string SYMBOL_HISTORY_PERIOD_ENDPOINT = "SymbolHistoryPeriod?Exchange={0}&Symbol={1}&Date={2}&Period={3}";

        private const string SYMBOL_HISTORY_PERIOD_DATE_RANGE_ENDPOINT = "SymbolHistoryPeriodByDateRange?Exchange={0}&Symbol={1}&StartDate={2}&EndDate={3}&Period={4}";

        private const string QUOTE_GET_ENDPOINT = "QuoteGet?Exchange={0}&Symbol={1}";

        private const string QUOTE_LIST_ENDPOINT = "QuoteList?Exchange={0}";

        private const string QUOTE_LIST_BY_DATE_ENDPOINT = "QuoteListByDate?Exchange={0}&QuoteDate={1}";

        private const string QUOTE_LIST_BY_DATE_PERIOD_ENDPOINT = "QuoteListByDatePeriod?Exchange={0}&QuoteDate={1}&Period={2}";

        public async Task<Exchange> ExchangeGetAsync(string exchange)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = string.Format(EXCHANGE_GET_ENDPOINT, exchange);

            var exchangeGetResponse = await Get<ExchangeGetResponse>(requestUrl).ConfigureAwait(false);

            return exchangeGetResponse.Exchange;
        }

        public async Task<List<Exchange>> ExchangeListAsync()
        {
            var exchangeListResponse = await Get<ExchangeListResponse>(EXCHANGE_LIST_ENDPOINT).ConfigureAwait(false);

            return exchangeListResponse.Exchanges.ExchangeList;
        }

        public async Task<Symbol> SymbolGetAsync(string exchange, string symbol, bool expandAbbreviations = false)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);

            var requestUrl = string.Format(SYMBOL_GET_ENDPOINT, exchange, symbol);

            var symbolGetResponse = await Get<SymbolGetResponse>(requestUrl).ConfigureAwait(false);

            if (!expandAbbreviations)
            {
                return symbolGetResponse.Symbol;
            }

            symbolGetResponse.Symbol = _mapper.Map<Symbol>(symbolGetResponse.Symbol);

            return symbolGetResponse.Symbol;
        }

        public async Task<List<Symbol>> SymbolListAsync(string exchange, bool expandAbbreviations = false)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = string.Format(SYMBOL_LIST_ENDPOINT, exchange);

            var symbolListResponse = await Get<SymbolListResponse>(requestUrl).ConfigureAwait(false);

            // remove null names it is invalid data
            symbolListResponse.Symbols.SymbolList.RemoveAll(x => string.IsNullOrWhiteSpace(x.Name));

            if (!expandAbbreviations)
                return symbolListResponse.Symbols.SymbolList;

            symbolListResponse.Symbols.SymbolList.ForEach((symbol) => _mapper.Map<Symbol>(symbol));            

            return symbolListResponse.Symbols.SymbolList;
        }

        public async Task<List<Quote>> SymbolHistoryAsync(string exchange, string symbol, string startDate)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);
            CheckIsNotNullOrWhitespace(nameof(startDate), startDate);

            var requestUrl = string.Format(SYMBOL_HISTORY_ENDPOINT, exchange, symbol, startDate);

            var symbolListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return symbolListResponse.Quotes.QuoteList;
        }

        public async Task<List<Quote>> SymbolHistoryPeriodAsync(string exchange, string symbol, string date, string period)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);
            CheckIsNotNullOrWhitespace(nameof(date), date);
            CheckIsNotNullOrWhitespace(nameof(period), period);

            var requestUrl = string.Format(SYMBOL_HISTORY_PERIOD_ENDPOINT, exchange, symbol, date, period);

            var symbolListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return symbolListResponse.Quotes.QuoteList;
        }

        public async Task<List<Quote>> SymbolHistoryPeriodByDateRangeAsync(string exchange, string symbol, string startDate, string endDate, string period)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);
            CheckIsNotNullOrWhitespace(nameof(startDate), startDate);
            CheckIsNotNullOrWhitespace(nameof(endDate), endDate);
            CheckIsNotNullOrWhitespace(nameof(period), period);

            var requestUrl = string.Format(SYMBOL_HISTORY_PERIOD_DATE_RANGE_ENDPOINT, exchange, symbol, startDate, endDate, period);

            var symbolListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return symbolListResponse.Quotes.QuoteList;
        }

        public async Task<Quote> QuoteGetAsync(string exchange, string symbol)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(symbol), symbol);

            var requestUrl = string.Format(QUOTE_GET_ENDPOINT, exchange, symbol);

            var quoteGetResponse = await Get<QuoteGetResponse>(requestUrl).ConfigureAwait(false);

            return quoteGetResponse.Quote;
        }

        public async Task<List<Quote>> QuoteListAsync(string exchange)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);

            var requestUrl = string.Format(QUOTE_LIST_ENDPOINT, exchange);

            var quoteListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return quoteListResponse.Quotes.QuoteList;
        }

        public async Task<List<Quote>> QuoteListByDateAsync(string exchange, string quoteDate)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(quoteDate), quoteDate);

            var requestUrl = string.Format(QUOTE_LIST_BY_DATE_ENDPOINT, exchange, quoteDate);

            var quoteListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return quoteListResponse.Quotes.QuoteList;
        }

        public async Task<List<Quote>> QuoteListByDatePeriodAsync(string exchange, string quoteDate, string period)
        {
            CheckIsNotNullOrWhitespace(nameof(exchange), exchange);
            CheckIsNotNullOrWhitespace(nameof(quoteDate), quoteDate);
            CheckIsNotNullOrWhitespace(nameof(period), period);

            var requestUrl = string.Format(QUOTE_LIST_BY_DATE_PERIOD_ENDPOINT, exchange, quoteDate, period);

            var quoteListResponse = await Get<QuoteListResponse>(requestUrl).ConfigureAwait(false);

            return quoteListResponse.Quotes.QuoteList;
        }
    }
}