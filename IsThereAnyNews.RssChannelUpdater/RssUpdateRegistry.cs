namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    public class RssUpdateRegistry : Registry
    {
        public RssUpdateRegistry()
        {
            this.Schedule<RssUpdateJob>()
                .NonReentrant()
                .ToRunNow()
                .AndEvery(15)
                .Minutes();
        }
    }
}