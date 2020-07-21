using Quartz;
using System;
using System.Threading.Tasks;

namespace Common.BackgroundTasks.TestApp
{
    [DisallowConcurrentExecution]
    public class ReceiveMessagesTask : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Executed");
            foreach (var p in context.MergedJobDataMap)
            {
                Console.WriteLine($"{p.Key} = {p.Value}");
            }
            return Task.CompletedTask;
        }
    }
}