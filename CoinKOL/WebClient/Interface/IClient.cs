using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinKOL.WebClient.Interface
{
    public interface IClient
    {
        public Task<HttpResponseMessage> GetJson();

    }
}
