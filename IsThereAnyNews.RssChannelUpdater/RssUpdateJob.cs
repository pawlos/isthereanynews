using FluentScheduler;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.RssChannelUpdater
{
    public class RssUpdateJob : IJob
    {
        private readonly IUpdateService updateService;

        public RssUpdateJob(IUpdateService updateService)
        {
            this.updateService = updateService;
        }

        public void Execute()
        {
            this.updateService.UpdateGlobalRss();
        }
    }
}
