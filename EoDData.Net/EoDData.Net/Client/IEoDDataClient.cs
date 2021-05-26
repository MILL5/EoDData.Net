using System.Threading.Tasks;

namespace EoDData.Net
{
    public interface IEoDDataClient
    {
        public Task<EXCHANGE> ExchangeGetAsync(string exchange);
    }
}