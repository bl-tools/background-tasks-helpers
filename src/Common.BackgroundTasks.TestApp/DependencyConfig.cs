using Common.QuartzNetBackgroundTaskHelpers;
using Common.QuartzNetBackgroundTaskHelpers.DependencyInjection;
using Common.QuartzNetBackgroundTaskHelpers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Common.BackgroundTasks.TestApp
{
    internal class DependencyConfig
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddScoped<ReceiveMessagesTask>();
            services.AddScoped<DynamicTasksSchedulerTask>();
            services.AddScoped<IDynamicTasksSchedulerConfigProvider, DynamicTasksSchedulerConfigProvider>();
            services.AddBackgroundTasks(context.Configuration, "BackgroundTasks");
        }
    }
}