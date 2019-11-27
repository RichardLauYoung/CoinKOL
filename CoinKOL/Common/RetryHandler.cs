﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoinKOL.Common
{
    public class RetryHandler : DelegatingHandler
    {
        public RetryHandler()
        {
        }

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
