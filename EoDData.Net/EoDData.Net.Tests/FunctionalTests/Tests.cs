using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static EoDData.Net.Tests.TestManager;

namespace EoDData.Net.Tests.FunctionalTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public async Task GetAsync()
        {
            var tickerDetailsResponse = await TestClient.ExchangeGetAsync("NASDAQ");
        }
    }
}