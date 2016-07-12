namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    public static class IsThereAnyNewsScheduler
    {
        public static void ScheduleRssUpdater()
        {
            JobManager.JobFactory = new AutofacRegistry();
            JobManager.Initialize(new RssUpdateRegistry());
        }
    }
}