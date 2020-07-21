using Common.QuartzNetBackgroundTaskHelpers.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.QuartzNetBackgroundTaskHelpers.HostedService
{
    internal class ScheduleStartupService : IHostedService
    {
        private readonly ILogger<IScheduler> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly BackgroundTasksConfigSection _backgroundTaskConfigs;
        private IScheduler _scheduler;

        public ScheduleStartupService(ILogger<IScheduler> logger,
                               ISchedulerFactory schedulerFactory,
                               IJobFactory jobFactory,
                               BackgroundTasksConfigSection backgroundTaskConfigs)
        {
            _logger = logger;
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
            _backgroundTaskConfigs = backgroundTaskConfigs;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            _scheduler.JobFactory = _jobFactory;

            foreach (var taskConfig in _backgroundTaskConfigs)
            {
                await ScheduleJob(taskConfig.Key, taskConfig.Value, cancellationToken);
            }

            await _scheduler.Start(cancellationToken);
            
            _logger.LogInformation("Job scheduler started.");
        }

        private async Task ScheduleJob(string taskName, BackgroundTaskConfig taskConfig, CancellationToken cancellationToken)
        {
            var job = JobBuilder
                      .Create(Type.GetType(taskConfig.TaskType))
                      .WithIdentity(taskName)
                      .UsingJobData(taskConfig.Parameters)
                      .Build();
            var trigger = TriggerBuilder
                          .Create()
                          .StartAt(DateTimeOffset.UtcNow.AddSeconds(taskConfig.StartDelaySeconds))
                          .WithSimpleSchedule(sb => sb.WithIntervalInSeconds(taskConfig.IntervalSeconds)
                                                      .RepeatForever())
                          .Build();
            var nextRun = await _scheduler.ScheduleJob(job, trigger, cancellationToken);
            _logger.LogInformation($"Job {job.Key} scheduled. Next run {nextRun}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _scheduler.Shutdown(cancellationToken);
            _logger.LogInformation("Graceful stop.");
        }
    }
}
