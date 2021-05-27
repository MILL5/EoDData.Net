using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EoDData.Net.Tests.TestManager;

namespace EoDData.Net.Tests.FunctionalTests
{
    [TestClass]
    public class Tests
    {
        private const string NASDAQ_EXCHANGE = "NASDAQ";

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
    }
}