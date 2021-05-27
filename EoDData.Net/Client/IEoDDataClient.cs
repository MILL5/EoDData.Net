using System.Threading.Tasks;

namespace EoDData.Net
{
    public interface IEoDDataClient
    {
        public Task<Exchange> ExchangeGetAsync(string exchange);
    }
}