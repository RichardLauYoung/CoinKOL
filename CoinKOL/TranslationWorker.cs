using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoinKOL.Services;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoinKOL
{
    /// <summary>
    /// 翻译服务（每五分钟，从数据库获取最新的数据进行翻译并保存）
    /// </summary>
    public class TranslationWorker : BackgroundService
    {
        private readonly ILogger<TranslationWorker> _logger;

        public TranslationWorker(ILogger<TranslationWorker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                AuthImplicit("qualified-gist-260503");

                //TODO
               var text = "We use Google Cloud Platform’s multi-regional deployment mode. This means that you can choose between different Google Cloud data center locations for each of your WordPress websites, which allows you to place your website in a geographical location closest to your visitors. This ensures low latency and blazing fast load times.";
                Console.WriteLine(text);
                TranslationService.Translation(text);
                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(300000, stoppingToken);
            }
        }

        /// <summary>
        /// 测试google授权
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public object AuthImplicit(string projectId)
        {
            try
            {
                
                string credentialPath = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
                if(credentialPath==null)
                {
                    string credential_path = @"/Users/mac/Library/Google/TranslationAPI-0a258748402c.json";
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credential_path);

                }
                Console.WriteLine(credentialPath);

                // If you don't specify credentials when constructing the client, the
                // client library will look for credentials in the environment.
                var credential = GoogleCredential.GetApplicationDefault();
                var storage = StorageClient.Create(credential);
                // Make an authenticated API request.
                var buckets = storage.ListBuckets(projectId);
                foreach (var bucket in buckets)
                {
                    Console.WriteLine(bucket.Name);
                }
            }
            catch (Google.GoogleApiException ex)
            {
                Console.WriteLine(ex.Message);
            }
           
            return null;
        }
    }
}
