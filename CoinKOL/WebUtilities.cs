using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace CoinKOL
{
    public class WebUtilities
    {
        public WebUtilities()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClient"></typeparam>
        /// <param name="services"></param>
        /// <param name="webSite"></param>
        /// <param name="baseUrl"></param>
        public static void Configure<TClient>(IServiceCollection services,string webSite, string baseUrl) where TClient:class
        {
            var registry = services.AddPolicyRegistry();

            var timeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
            var longTimeout = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

            registry.Add("regular", timeout);
            registry.Add("long", longTimeout);

            services.AddHttpClient(webSite, c =>
            {
                c.BaseAddress = new Uri(baseUrl);
                c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // GitHub API versioning
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample"); // GitHub requires a user-agent
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

            .AddHttpMessageHandler(() => new RetryHandler()) // Retry requests to github using our retry handler
            .AddTypedClient<TClient>();
        }

        private class RetryHandler : DelegatingHandler
        {
            public int RetryCount { get; set; } = 5;

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                for (var i = 0; i < RetryCount; i++)
                {
                    try
                    {
                        return await base.SendAsync(request, cancellationToken);
                    }
                    catch (HttpRequestException) when (i == RetryCount - 1)
                    {
                        throw;
                    }
                    catch (HttpRequestException)
                    {
                        // Retry
                        await Task.Delay(TimeSpan.FromMilliseconds(50));
                    }
                }

                // Unreachable.
                throw null;
            }
        }
    }
}
