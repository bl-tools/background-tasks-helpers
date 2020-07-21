using Quartz;

namespace Common.QuartzNetBackgroundTaskHelpers.Config
{
    internal class BackgroundTaskConfig
    {
        public BackgroundTaskConfig()
        {
            Parameters = new JobDataMap();
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string TaskType { get; set; }
        public int IntervalSeconds { get; set; }
        public int StartDelaySeconds { get; set; }
        public JobDataMap Parameters { get; set; }
    }
}
