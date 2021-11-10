using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static EoDData.Net.Tests.TestManager;

namespace EoDData.Net.Tests.FunctionalTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class Tests
    {
        private const string NASDAQ_EXCHANGE = "NASDAQ";

        private const string MICROSOFT_SYMBOL = "MSFT";

        private const string TICKER_AAPL = "AAPL";

        private const string APPLE_EXPANDED = "Apple Incorporated";

        private const string VALID_DATE_1 = "20200101";

        private const string VALID_DATE_2 = "20200102";

        private const string PERIOD_DAY = "d";

        private readonly string EoDDataStartDate;

        private readonly string EoDDataEndDate;

        public Tests()
        {
            EoDDataStartDate = DateTime.Now.AddYears(-3).AddDays(1).ToString("yyyyMMdd");

            EoDDataEndDate = DateTime.Now.ToString("yyyyMMdd");
        }

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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            Assert.AreEqual(symbol.NormalizedCode, TICKER_AAPL);
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
        public async Task SymbolListNullNameAsync()
        {
            var symbols = await TestClient.SymbolListAsync("NYSE");

            Assert.IsNotNull(symbols);
            Assert.IsTrue(symbols.Any());

            var emptyNameList = symbols.LastOrDefault(x => string.IsNullOrWhiteSpace(x.Name));

            Assert.IsNull(emptyNameList);
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
                async () => await TestClient.SymbolHistoryPeriodAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, "adsva"));
        }

        [TestMethod]
        public async Task SymbolHistoryPeriodByDateRangeSucceedsAsync()
        {
            var quotes = await TestClient.SymbolHistoryPeriodByDateRangeAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, VALID_DATE_1, VALID_DATE_2, PERIOD_DAY);

            Assert.IsNotNull(quotes);
            Assert.AreEqual(3, quotes.Count);
        }

        [TestMethod]
        public async Task SymbolHistoryPeriodByEntireDateRangeSucceedsAsync()
        {
            var quotes = await TestClient.SymbolHistoryPeriodByDateRangeAsync(NASDAQ_EXCHANGE, MICROSOFT_SYMBOL, EoDDataStartDate, EoDDataEndDate, PERIOD_DAY);

            Assert.IsNotNull(quotes);
            Assert.IsTrue(quotes.Any());
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
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
            await Assert.ThrowsExceptionAsync<EoDDataException>(
                async () => await TestClient.QuoteListByDatePeriodAsync(exchange, VALID_DATE_1, period));
        }

        [TestMethod]
        public async Task InvalidLoginUserAsync()
        {
            var settings = new EoDDataSettings(
                Environment.GetEnvironmentVariable("EoDDataUsername"),
                "InvalidUser",
                "InvalidPassword");

            var dependency = new EoDDataDependencies(
                settings,
                Dependencies.HttpClientFactory,
                Dependencies.Mapper);

            var client = new EoDDataClient(dependency);

            await Assert.ThrowsExceptionAsync<EoDDataException>(
                async () => await client.ExchangeGetAsync(NASDAQ_EXCHANGE));
        }

        [TestMethod]
        public async Task InvalidTokenAndRegenerateUserAsync()
        {
            var settings = new EoDDataSettings(
                Environment.GetEnvironmentVariable("EoDDataBaseUrl"),
                Environment.GetEnvironmentVariable("EoDDataUsername"),
                Environment.GetEnvironmentVariable("EoDDataPassword"));

            BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;
            PropertyInfo pi = settings.GetType().GetProperty("ApiLoginToken", bf);
            pi.SetValue(settings, "TOKEN01", null);

            var dependency = new EoDDataDependencies(
                settings,
                Dependencies.HttpClientFactory,
                Dependencies.Mapper);

            var client = new EoDDataClient(dependency);          

            var exchanges = await client.ExchangeListAsync();

            Assert.IsNotNull(exchanges);
            Assert.IsTrue(exchanges.Any());

            var ignored = new List<string>() { nameof(Exchange.Suffix) };
            AssertAllPropertiesNotNull(exchanges.First(), ignored);
        }

        [DataTestMethod]
        [DataRow("Arlington Asset Investment Corp [Aaic/Pb]", "AAI-B", "AAICpB")]
        [DataRow("Accelerate Acquisition Corp [Aaqc.U]", "AAQ.U", "AAQC.U")]
        [DataRow("Acropolis Infrastructure Acquisition WT [Acro/W]", "ACR.W", "ACRO.WS")]
        [DataRow("Aeva Technologies Inc WT [Aeva/S]", "AEV.W", "AEVA.S")]
        [DataRow("Ashford Hospitality TR Inc [Aht/Pi]", "AHT-I", "AHTpI")]
        [DataRow("Tidewater Inc [Tdw/Wa]", "TDW.A", "TDW.WS.A")]
        public void NormalizeSymbolTestSuccess(string name, string code, string expected)
        {            
            var actual = EoDDataClient.NormalizeSymbol(name, code);
            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DataRow(NASDAQ_EXCHANGE)]
        [DataRow("AMEX")]
        [DataRow("NYSE")]
        public async Task SplitsByExchangeAsync(string exchange)
        {
            var splits = await TestClient.SplitsByExchangeAsync(exchange);

            Assert.IsNotNull(splits);
            Assert.IsTrue(splits.Any());

            AssertAllPropertiesNotNull(splits.First());
        }

        [TestMethod]
        public async Task SplitsByExchangeNullExchangeAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(
                async () => await TestClient.SplitsByExchangeAsync(string.Empty));            
        }

        [TestMethod]
        public async Task QuoteListNotNullValuesAsync()
        {
            var quotes = await TestClient.QuoteListAsync(NASDAQ_EXCHANGE);

            var emptyQuotes = quotes.LastOrDefault(x => x.Symbol == null || x.Name == null);
            
            Assert.IsNull(emptyQuotes);
        }        
    }    
}