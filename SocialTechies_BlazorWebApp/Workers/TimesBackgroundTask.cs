using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialTechies_BlazorWebApp.Workers
{
    public class TimesBackgroundTask : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimesBackgroundTask> _logger;
        private Timer _timer;
        public TimesBackgroundTask(
            ILogger<TimesBackgroundTask> logger)
        {
            try
            {
                _logger = logger;
            }
            catch (Exception)
            { }
        }

        public virtual Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }

        public virtual Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
