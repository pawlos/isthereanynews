using FluentScheduler;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.Services;
using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.RssChannelUpdater
{
    public class RssUpdateJob : IJob
    {
        private readonly IUpdateService updateService;

        public RssUpdateJob()
        {
            var itandb = new ItanDatabaseContext();
            var updateRepository = new UpdateRepository(itandb);
            var rssEntriesRepository = new RssEntriesRepository(itandb);
            var rssChannelsRepository = new RssChannelsRepository(itandb);
            var rssChannelUpdateRepository = new RssChannelUpdateRepository(itandb);
            this.updateService = new UpdateService(updateRepository,
                rssEntriesRepository, rssChannelsRepository, rssChannelUpdateRepository);
        }

        public void Execute()
        {
            this.updateService.UpdateGlobalRss();
        }
    }
}
