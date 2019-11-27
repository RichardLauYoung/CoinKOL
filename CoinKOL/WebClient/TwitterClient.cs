using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Polly;
using CoinKOL.Common;

namespace CoinKOL.WebClient
{
    public class TwitterClient
    {
        public HttpClient HttpClient { get; }

        private const string website = "twitter";
        private const string baseAddress = "https://api.github.com/";
        private const string accept = "application/vnd.github.v3+json";
        private const string userAgent = "HttpClientFactory-Sample";

        public TwitterClient(HttpClient httpClient)
        {
            HttpClient = httpClient;

        }

        /// <summary>
        /// Configure the http service
        /// </summary>
        /// <param name="services">service factory</param>
        public static void Configure(IServiceCollection services)
        {
            var registry = services.AddPolicyRegistry();

            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            var longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            registry.Add("regular", timeout);
            registry.Add("long", longTimeout);

            services.AddHttpClient(website, c =>
            {
                c.BaseAddress = new Uri(baseAddress);

                c.DefaultRequestHeaders.Add("Accept", accept); // twitter API versioning
                c.DefaultRequestHeaders.Add("User-Agent", userAgent); // twitter requires a user-agent
            })

            // Build a totally custom policy using any criteria
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))

            // Use a specific named policy from the registry. Simplest way, policy is cached for the
            // lifetime of the handler.
            .AddPolicyHandlerFromRegistry("regular")

            // Run some code to select a policy based on the request
            .AddPolicyHandler((request) =>
            {
                return request.Method == HttpMethod.Get ? timeout : longTimeout;
            })

            // Run some code to select a policy from the registry based on the request
            .AddPolicyHandlerFromRegistry((reg, request) =>
            {
                return request.Method == HttpMethod.Get ?
                    reg.Get<IAsyncPolicy<HttpResponseMessage>>("regular") :
                    reg.Get<IAsyncPolicy<HttpResponseMessage>>("long");
            })

            // Build a policy that will handle exceptions, 408s, and 500s from the remote server
            .AddTransientHttpErrorPolicy(p => p.RetryAsync())

            .AddHttpMessageHandler(() => new RetryHandler()) // Retry requests to twitter using our retry handler
            .AddTypedClient<TwitterClient>();
        }



        /// <summary>
        ///  Gets the list of services on Twitter
        /// </summary>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetJson()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/");

            var response = await HttpClient.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return response;
        }

        

    }
}
