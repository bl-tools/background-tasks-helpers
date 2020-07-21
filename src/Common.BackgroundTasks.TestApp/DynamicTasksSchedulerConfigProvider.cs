using Common.QuartzNetBackgroundTaskHelpers.Config;
using Common.QuartzNetBackgroundTaskHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.BackgroundTasks.TestApp
{
    public class DynamicTasksSchedulerConfigProvider : IDynamicTasksSchedulerConfigProvider
    {
        private const string TaskType = "Common.BackgroundTasks.TestApp.ReceiveMessagesTask,Common.BackgroundTasks.TestApp";

        public Task<IReadOnlyCollection<JobConfig>> GetJobConfigs()
        {
            return Task.FromResult((IReadOnlyCollection<JobConfig>) new List<JobConfig>
            {
                new JobConfig
                {
                    IsJobActive = DateTime.Now.Second > 45,
                    IntervalSeconds = 4,
                    StartDelaySeconds = 1,
                    Name = "Q1",
                    TaskType = TaskType,
                    Parameters = new Dictionary<string, string>
                    {
                        ["p1"] = "v1"
                    }
                },
                new JobConfig
                {
                    IsJobActive = true,
                    IntervalSeconds = 4,
                    StartDelaySeconds = 2,
                    Name = "Q2",
                    TaskType = TaskType,
                    Parameters = new Dictionary<string, string>
                    {
                        ["p2"] = "v2"
                    }
                }
            });
        }
    }
}