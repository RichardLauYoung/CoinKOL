using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await Task.Delay(30000, stoppingToken);
            }
        }
    }
}
