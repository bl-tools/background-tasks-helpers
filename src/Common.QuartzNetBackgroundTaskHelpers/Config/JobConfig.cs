using System.Collections.Generic;

namespace Common.QuartzNetBackgroundTaskHelpers.Config
{
    public class JobConfig
    {
        public string Name { get; set; }

        public string TaskType { get; set; }

        public int StartDelaySeconds { get; set; }

        public int IntervalSeconds { get; set; }

        public bool IsJobActive { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}