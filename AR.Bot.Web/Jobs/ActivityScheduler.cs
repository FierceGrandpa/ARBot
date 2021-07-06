using Quartz;
using Quartz.Impl;

namespace AR.Bot.Jobs
{
    public class ActivityScheduler
    {
        public static async void Start()
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();

            var job = JobBuilder.Create<ActivitySender>().Build();
        }
    }
}