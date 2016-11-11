namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.HtmlStrip;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.Services.Implementation;

    public class RssUpdateJob : IJob
    {
        private readonly IUpdateService updateService;

        public RssUpdateJob()
        {
            var itanDatabaseContext = new ItanDatabaseContext();
            var updateRepository = new UpdateRepository(itanDatabaseContext);
            var rssEntriesRepository = new RssEntriesRepository(itanDatabaseContext);
            var rssChannelsRepository = new RssChannelsRepository(itanDatabaseContext);
            var rssChannelUpdateRepository = new RssChannelUpdateRepository(itanDatabaseContext);
            var htmlStripper = new HtmlStripper();

            var configureMapper = IsThereAnyNewsAutomapper.ConfigureMapper();
            ISyndicationFeedAdapter syndicationFeedAdapter = new SyndicationFeedAdapter(configureMapper);

            this.updateService = new UpdateService(
                updateRepository,
                rssEntriesRepository,
                rssChannelsRepository,
                rssChannelUpdateRepository,
                syndicationFeedAdapter,
                htmlStripper);
        }

        public void Execute()
        {
            this.updateService.UpdateGlobalRss();
        }
    }
}
