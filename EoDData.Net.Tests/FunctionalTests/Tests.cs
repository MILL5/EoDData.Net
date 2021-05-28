using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EoDData.Net.Tests.TestManager;

namespace EoDData.Net.Tests.FunctionalTests
{
    [TestClass]
    public class Tests
    {
        private const string NASDAQ_EXCHANGE = "NASDAQ";

        private const string MICROSOFT_SYMBOL = "MSFT";

        [TestMethod]
        public async Task ExchangeGetSucceedsAsync()
        {
            var exchange = await TestClient.ExchangeGetAsync(NASDAQ_EXCHANGE);

            var ignored = new List<string>() { nameof(Exchange.Suffix) };
            AssertAllPropertiesNotNull(exchange, ignored);
        }

        [TestMethod]
        public async Task ExchangeGetBadExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.ExchangeGetAsync("Fipadippitybop"));
        }

        [TestMethod]
        public async Task ExchangeGetNoExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.ExchangeGetAsync(string.Empty));
        }

        [TestMethod]
        public async Task ExchangeListSucceedsAsync()
        {
            var exchanges = await TestClient.ExchangeListAsync();

            Assert.IsNotNull(exchanges);
            Assert.IsTrue(exchanges.Any());

            var ignored = new List<string>() { nameof(Exchange.Suffix) };
            AssertAllPropertiesNotNull(exchanges.First(), ignored);
        }

        [TestMethod]
        public async Task SymbolGetSucceedsAsync()
        {
            var symbol = await TestClient.SymbolGetAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL);

            AssertAllPropertiesNotNull(symbol);
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow(NASDAQ_EXCHANGE, "")]
        [DataRow("", MICROSOFT_SYMBOL)]
        public async Task SymbolGetNoExSymbAsync(string exchange, string symbol)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                    async () => await TestClient.SymbolGetAsync(exchange, symbol));
        }

        [DataTestMethod]
        [DataRow(NASDAQ_EXCHANGE, "beeboop")]
        [DataRow("beeboop", MICROSOFT_SYMBOL)]
        public async Task SymbolGetNonExistentExSymbAsync(string exchange, string symbol)
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                    async () => await TestClient.SymbolGetAsync(exchange, symbol));
        }

        [TestMethod]
        public async Task SymbolListSucceedsAsync()
        {
            var symbols = await TestClient.SymbolListAsync(NASDAQ_EXCHANGE);

            Assert.IsNotNull(symbols);
            Assert.IsTrue(symbols.Any());

            AssertAllPropertiesNotNull(symbols.First());
        }
    }
}