namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.Services.Implementation;

    public class RssUpdateJob : IJob
    {
        private readonly IUpdateService updateService;

        public RssUpdateJob()
        {
            var configureMapper = IsThereAnyNewsAutomapper.ConfigureMapper();

            var itanDatabaseContext = new ItanDatabaseContext();
            var updateRepository = new EntityRepository(itanDatabaseContext, null);

            ISyndicationFeedAdapter syndicationFeedAdapter = new SyndicationFeedAdapter(configureMapper);

            this.updateService = new UpdateService(
                syndicationFeedAdapter,
                configureMapper,
                updateRepository);
        }

        public void Execute()
        {
            this.updateService.UpdateGlobalRss();
        }
    }
}
