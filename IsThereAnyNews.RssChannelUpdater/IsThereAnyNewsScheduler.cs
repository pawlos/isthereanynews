using FluentScheduler;

namespace IsThereAnyNews.RssChannelUpdater
{
    public static class IsThereAnyNewsScheduler
    {
        public static void ScheduleRssUpdater()
        {
            JobManager.JobFactory = new AutofacRegistry();
            JobManager.Initialize(new RssUpdateRegistry());
        }
    }
}