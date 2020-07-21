using Common.QuartzNetBackgroundTaskHelpers.Config;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.QuartzNetBackgroundTaskHelpers.Interfaces
{
    public interface IDynamicTasksSchedulerConfigProvider
    {
        Task<IReadOnlyCollection<JobConfig>> GetJobConfigs();
    }
}