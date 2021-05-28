﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace EoDData.Net
{
    public interface IEoDDataClient
    {
        public Task<Exchange> ExchangeGetAsync(string exchange);

        public Task<List<Exchange>> ExchangeListAsync();

        public Task<Symbol> SymbolGetAsync(string exchange, string symbol);

        public Task<List<Symbol>> SymbolListAsync(string exchange);
    }
}