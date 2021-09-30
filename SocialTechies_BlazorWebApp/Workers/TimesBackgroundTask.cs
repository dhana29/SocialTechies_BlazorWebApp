using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SocialTechies_BlazorWebApp.Data.Aws;
using SocialTechies_BlazorWebApp.Logger;
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
        int period = 3600;
        DateTime startTime = DateTime.Now.AddMinutes(30);
        DateTime endTime = DateTime.Now;
        AwsService aws;
        private readonly IConfiguration _Configuration;
        public TimesBackgroundTask(
            ILogger<TimesBackgroundTask> logger,
            IConfiguration configuration)
        {
            try
            {
                aws = new AwsService();
                _logger = logger;
                _Configuration = configuration;
            }
            catch (Exception)
            { }
        }

        public virtual Task StartAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(10));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);
            var autoscalingGroup = _Configuration["autoscalingGroup"];
            Task<EcsMetrics.CpuUtilization> cpuUtilization = aws.GetCpuUtilizationForAutoscalingGroup(autoscalingGroup, period, startTime, endTime);
            if (cpuUtilization?.Result?.Datapoints?.OrderByDescending(t => t.TimeStamp).First().Maximum > 5)
            {
                //Call Slake 
                //TODO: Add Slake URL
                CloudLogger.logSlack($"{autoscalingGroup} group utilized > 5. Value is {cpuUtilization.Result.Datapoints.OrderByDescending(t => t.TimeStamp).First().Maximum}","");
            }
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
