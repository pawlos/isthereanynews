using FluentScheduler;

namespace IsThereAnyNews.RssChannelUpdater
{
    public class RssUpdateRegistry : Registry
    {
        public RssUpdateRegistry()
        {
            Schedule<RssUpdateJob>()
                .NonReentrant()
                .ToRunNow()
                .AndEvery(1)
                .Minutes();

            Schedule<RssReadersJob>()
                .NonReentrant()
                .ToRunNow()
                .AndEvery(1)
                .Minutes();
        }
    }
}