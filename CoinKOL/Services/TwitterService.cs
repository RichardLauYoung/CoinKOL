using System;
using System.Threading.Tasks;
using CoinKOL.WebClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CoinKOL.Services
{
    public class TwitterService
    {
        public TwitterService()
        {
            
        }

        public static async Task Run()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(b =>
                {
                    b.AddFilter((category, level) => true); // Spam the world with logs.

                    // Add console logger so we can see all the logging produced by the client by default.
                    b.AddConsole(c => c.IncludeScopes = true);
                });

            TwitterClient.Configure(serviceCollection);

            var services = serviceCollection.BuildServiceProvider();

            var twitter = services.GetRequiredService<TwitterClient>();
        }
    }
}
