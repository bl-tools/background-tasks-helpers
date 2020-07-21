using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Spi;

namespace Common.QuartzNetBackgroundTaskHelpers.Implementations
{
    internal sealed class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobFactory> _logger;

        public JobFactory(IServiceProvider serviceProvider, ILogger<JobFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var job = (IJob)scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);

                return new ScopedJobWrapper(job, scope);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to resolve job from service collection. ");
                throw;
            }
        }

        public void ReturnJob(IJob job)
        {
            (job as ScopedJobWrapper)?.Dispose();
        }

        private sealed class ScopedJobWrapper : IJob, IDisposable
        {
            private readonly IJob _job;
            private readonly IServiceScope _scope;

            public ScopedJobWrapper(IJob job, IServiceScope scope)
            {
                _job = job;
                _scope = scope;
            }

            public Task Execute(IJobExecutionContext context)
            {
                return _job.Execute(context);
            }

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}