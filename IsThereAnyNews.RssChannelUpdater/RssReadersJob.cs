namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.Services.TestSupport;

    public class RssReadersJob : IJob
    {
        private readonly TestService testService;

        public RssReadersJob()
        {
            var itanDatabaseContext = new ItanDatabaseContext();
            var userRepository = new UserRepository(itanDatabaseContext);
            var rssChannelsRepository = new RssChannelsRepository(itanDatabaseContext);
            var rssChannelsSubscriptionsRepository = new RssChannelsSubscriptionsRepository(itanDatabaseContext);
            var rssEntriesToReadRepository = new RssEntriesToReadRepository(itanDatabaseContext);
            var rssEventRepository = new RssEventRepository(itanDatabaseContext);
            this.testService = new TestService(userRepository, rssChannelsRepository, rssChannelsSubscriptionsRepository, rssEntriesToReadRepository, rssEventRepository, itanDatabaseContext);
        }

        public void Execute()
        {
            this.testService.ReadRandomRssEvent();
        }
    }
}