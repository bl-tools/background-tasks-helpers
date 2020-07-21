using Common.QuartzNetBackgroundTaskHelpers.Interfaces;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.QuartzNetBackgroundTaskHelpers
{
    [DisallowConcurrentExecution]
    public class DynamicTasksSchedulerTask : IJob
    {
        private readonly IDynamicTasksSchedulerConfigProvider _jobConfigProvider;

        public DynamicTasksSchedulerTask(IDynamicTasksSchedulerConfigProvider jobConfigProvider)
        {
            _jobConfigProvider = jobConfigProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var groupName = context.JobDetail.Key.Name;
            var tasksToSchedule = await _jobConfigProvider.GetJobConfigs();
            var taskNamesHaveDuplicates = tasksToSchedule.GroupBy(x => x.Name).Any(g => g.Count() > 1);

            var scheduledJobKeys = await context.Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
            var notPresentInConfigJobKeys = scheduledJobKeys.Where(job => tasksToSchedule.All(task => task.Name != job.Name)).ToList();
            await context.Scheduler.DeleteJobs(notPresentInConfigJobKeys);

            foreach (var task in tasksToSchedule)
            {
                var jobKey = new JobKey(task.Name, groupName);
                if (!task.IsJobActive || taskNamesHaveDuplicates)
                {
                    await context.Scheduler.DeleteJob(jobKey);
                    continue;
                }

                var currentJob = await context.Scheduler.GetJobDetail(jobKey);
                if (currentJob != null)
                {
                    continue;
                }

                var job = JobBuilder
                    .Create(Type.GetType(task.TaskType))
                    .WithIdentity(jobKey)
                    .UsingJobData(new JobDataMap(task.Parameters))
                    .Build();
                var trigger = TriggerBuilder.Create()
                    .StartAt(DateTimeOffset.UtcNow.AddSeconds(task.StartDelaySeconds))
                    .WithSimpleSchedule(sb => sb.WithIntervalInSeconds(task.IntervalSeconds)
                        .RepeatForever())
                    .Build();
                await context.Scheduler.ScheduleJob(job, trigger);
            }
        }
    }
}
