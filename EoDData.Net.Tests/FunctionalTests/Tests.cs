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

        private const string TICKER_AAPL = "AAPL";

        private const string APPLE_EXPANDED = "Apple Incorporated";

        private const string VALID_DATE_1 = "20200101";

        private const string VALID_DATE_2 = "20200102";

        private const string PERIOD_DAY = "d";

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

        [TestMethod]
        public async Task SymbolGetWithExpansionSucceedsAsync()
        {
            var symbol = await TestClient.SymbolGetAsync(NASDAQ_EXCHANGE, TICKER_AAPL, true);

            AssertAllPropertiesNotNull(symbol);

            Assert.IsTrue(symbol.Name == APPLE_EXPANDED);
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

        [TestMethod]
        public async Task SymbolListWithExpansionSucceedsAsync()
        {
            var symbols = await TestClient.SymbolListAsync(NASDAQ_EXCHANGE, true);

            Assert.IsNotNull(symbols);
            Assert.IsTrue(symbols.Any());

            AssertAllPropertiesNotNull(symbols.First());

            var aaplTicker = symbols.First(x => x.Name == APPLE_EXPANDED);

            Assert.IsNotNull(aaplTicker);
        }

        [TestMethod]
        public async Task SymbolListNoExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.ExchangeGetAsync(string.Empty));
        }

        [TestMethod]
        public async Task SymbolListBadExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.ExchangeGetAsync("Fipadippitybop"));
        }

        [TestMethod]
        public async Task SymbolHistorySucceedsAsync()
        {
            var quote = await TestClient.SymbolHistoryAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1);

            Assert.IsNotNull(quote);
            Assert.IsTrue(quote.Count > 1);
        }

        [DataTestMethod]
        [DataRow("", "", "")]
        [DataRow(NASDAQ_EXCHANGE, "", VALID_DATE_1)]
        [DataRow("", MICROSOFT_SYMBOL, VALID_DATE_1)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, "")]
        public async Task SymbolHistoryEmptyCheckAsync(string exchange, string symbol, string date)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.SymbolHistoryAsync(exchange, symbol, date));
        }

        [TestMethod]
        public async Task SymbolHistoryPeriodSucceedsAsync()
        {
            var quote = await TestClient.SymbolHistoryPeriodAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, PERIOD_DAY);

            Assert.IsNotNull(quote);
            Assert.AreEqual(1, quote.Count);
        }

        [DataTestMethod]
        [DataRow("", "", "", "")]
        [DataRow(NASDAQ_EXCHANGE, "", VALID_DATE_1, PERIOD_DAY)]
        [DataRow("", MICROSOFT_SYMBOL, VALID_DATE_1, PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, "", PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, "")]
        public async Task SymbolHistoryPeriodEmptyCheckAsync(string exchange, string symbol, string date, string period)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.SymbolHistoryPeriodAsync(exchange, symbol, date, period));
        }

        public async Task SymbolHistoryPeriodBadPeriodSymbAsync(string exchange, string symbol, string date)
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.SymbolHistoryPeriodAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, "adsva"));
        }

        [TestMethod]
        public async Task SymbolHistoryPeriodByDateRangeSucceedsAsync()
        {
            var quote = await TestClient.SymbolHistoryPeriodByDateRangeAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, VALID_DATE_2, PERIOD_DAY);

            Assert.IsNotNull(quote);
            Assert.AreEqual(3, quote.Count);
        }

        [DataTestMethod]
        [DataRow("", "", "", "", "")]
        [DataRow(NASDAQ_EXCHANGE, "", VALID_DATE_1, VALID_DATE_2, PERIOD_DAY)]
        [DataRow("", MICROSOFT_SYMBOL, VALID_DATE_1, VALID_DATE_2, PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, "", VALID_DATE_2, PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, "", PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, VALID_DATE_2, "")]
        public async Task SymbolHistoryPeriodByDateRangeEmptyCheckAsync(string exchange, string symbol, string startDate, string endDate, string period)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.SymbolHistoryPeriodByDateRangeAsync(exchange, symbol, startDate, endDate, period));
        }

        public async Task SymbolHistoryPeriodByDateRangeBadPeriodSymbAsync()
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.SymbolHistoryPeriodByDateRangeAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, VALID_DATE_2, "adsva"));
        }

        [TestMethod]
        public async Task QuoteGetSucceedsAsync()
        {
            var quote = await TestClient.QuoteGetAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL);

            AssertAllPropertiesNotNull(quote);
        }

        [DataTestMethod]
        [DataRow("", "")]
        [DataRow(NASDAQ_EXCHANGE, "")]
        [DataRow("", MICROSOFT_SYMBOL)]
        public async Task QuoteGetNoExSymbAsync(string exchange, string symbol)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                    async () => await TestClient.QuoteGetAsync(exchange, symbol));
        }

        [DataTestMethod]
        [DataRow(NASDAQ_EXCHANGE, "beeboop")]
        [DataRow("beeboop", MICROSOFT_SYMBOL)]
        public async Task QuoteGetNonExistentExSymbAsync(string exchange, string symbol)
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                    async () => await TestClient.QuoteGetAsync(exchange, symbol));
        }

        [TestMethod]
        public async Task QuoteListSucceedsAsync()
        {
            var quotes = await TestClient.QuoteListAsync(NASDAQ_EXCHANGE);

            Assert.IsNotNull(quotes);
            Assert.IsTrue(quotes.Any());

            AssertAllPropertiesNotNull(quotes.First());
        }

        [TestMethod]
        public async Task QuoteListEmptyExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.QuoteListAsync(""));
        }

        [TestMethod]
        public async Task QuoteListBadExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.QuoteListAsync("Fipadippitybop"));
        }

        [TestMethod]
        public async Task QuoteListByDateSucceedsAsync()
        {
            var quotes = await TestClient.QuoteListByDateAsync(NASDAQ_EXCHANGE, VALID_DATE_1);

            Assert.IsNotNull(quotes);
            Assert.IsTrue(quotes.Any());

            AssertAllPropertiesNotNull(quotes.First());
        }

        [DataTestMethod]
        [DataRow(NASDAQ_EXCHANGE, "")]
        [DataRow("", VALID_DATE_1)]
        public async Task QuoteListByDateEmptyExDateAsync(string exchange, string quoteDate)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.QuoteListByDateAsync(exchange, quoteDate));
        }

        [TestMethod]
        public async Task QuoteListByDateBadExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.QuoteListByDateAsync("Fipadippitybop", VALID_DATE_1));
        }

        [TestMethod]
        public async Task QuoteListByDatePeriodSucceedsAsync()
        {
            var quotes = await TestClient.QuoteListByDatePeriodAsync(NASDAQ_EXCHANGE, VALID_DATE_1, PERIOD_DAY);

            Assert.IsNotNull(quotes);
            Assert.IsTrue(quotes.Any());

            AssertAllPropertiesNotNull(quotes.First());
        }

        [DataTestMethod]
        [DataRow("", "", "")]
        [DataRow("", VALID_DATE_1, PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, "", PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, VALID_DATE_1, "")]
        public async Task QuoteListByDatePeriodEmptyExDatePeriodAsync(string exchange, string quoteDate, string period)
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.QuoteListByDatePeriodAsync(exchange, quoteDate, period));
        }

        [DataTestMethod]
        [DataRow("Fipadippitybop", PERIOD_DAY)]
        [DataRow(NASDAQ_EXCHANGE, "Fipadippitybop")]
        public async Task QuoteListByDatePeriodBadExchangePeriodAsync(string exchange, string period)
        {
            await Assert.ThrowsExceptionAsync<EoDDataHttpException>(
                async () => await TestClient.QuoteListByDatePeriodAsync(exchange, VALID_DATE_1, period));
        }
    }
}