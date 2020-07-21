using Microsoft.Extensions.Hosting;

namespace Common.BackgroundTasks.TestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(DependencyConfig.ConfigureServices)
        ;
    }
}
