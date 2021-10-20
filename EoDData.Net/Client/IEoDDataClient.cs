using System.Collections.Generic;
using System.Threading.Tasks;

namespace EoDData.Net
{
    public interface IEoDDataClient
    {
        Task<Exchange> ExchangeGetAsync(string exchange);

        Task<List<Exchange>> ExchangeListAsync();

        Task<Symbol> SymbolGetAsync(string exchange, string symbol, bool expandAbbreviations = false);

        Task<List<Symbol>> SymbolListAsync(string exchange, bool expandAbbreviations = false);

        Task<List<Quote>> SymbolHistoryAsync(string exchange, string symbol, string startDate);

        Task<List<Quote>> SymbolHistoryPeriodAsync(string exchange, string symbol, string date, string period);

        Task<List<Quote>> SymbolHistoryPeriodByDateRangeAsync(string exchange, string symbol, string startDate, string endDate, string period);

        Task<Quote> QuoteGetAsync(string exchange, string symbol);

        Task<List<Quote>> QuoteListAsync(string exchange);

        Task<List<Quote>> QuoteListByDateAsync(string exchange, string quoteDate);

        Task<List<Quote>> QuoteListByDatePeriodAsync(string exchange, string quoteDate, string period);

        Task<List<Split>> SplitsByExchangeAsync(string exchange);
    }
}