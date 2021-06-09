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

        Task<Quote> QuoteGetAsync(string exchange, string symbol);

        Task<List<Quote>> QuoteListAsync(string exchange, string date);
    }
}