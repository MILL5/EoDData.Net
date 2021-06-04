using System.Collections.Generic;
using System.Threading.Tasks;

namespace EoDData.Net
{
    public interface IEoDDataClient
    {
        public Task<Exchange> ExchangeGetAsync(string exchange);

        public Task<List<Exchange>> ExchangeListAsync();

        public Task<Symbol> SymbolGetAsync(string exchange, string symbol);

        public Task<List<Symbol>> SymbolListAsync(string exchange);

        public Task<List<Quote>> SymbolHistoryAsync(string exchange, string symbol, string startDate);

        public Task<List<Quote>> SymbolHistoryPeriodAsync(string exchange, string symbol, string date, string period);

        public Task<List<Quote>> SymbolHistoryPeriodByDateRangeAsync(string exchange, string symbol, string startDate, string endDate, string period);

        public Task<Quote> QuoteGetAsync(string exchange, string symbol);

        public Task<List<Quote>> QuoteListAsync(string exchange);

        public Task<List<Quote>> QuoteListByDateAsync(string exchange, string quoteDate);

        public Task<List<Quote>> QuoteListByDatePeriodAsync(string exchange, string quoteDate, string period);
    }
}