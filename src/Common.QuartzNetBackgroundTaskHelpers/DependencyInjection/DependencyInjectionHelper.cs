using Common.QuartzNetBackgroundTaskHelpers.Config;
using Common.QuartzNetBackgroundTaskHelpers.HostedService;
using Common.QuartzNetBackgroundTaskHelpers.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Common.QuartzNetBackgroundTaskHelpers.DependencyInjection
{
    public static class DependencyInjectionHelper
    {
        public static IServiceCollection AddBackgroundTasks(this IServiceCollection services, IConfiguration configuration, string configSection)
        {
            var config = new BackgroundTasksConfigSection();
            configuration.GetSection(configSection).Bind(config);
            services.AddSingleton(config);

            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            services.AddHostedService<ScheduleStartupService>();

            return services;
        }
    }
}
