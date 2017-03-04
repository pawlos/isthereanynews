namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;

    public class RssReadersJob : IJob
    {

        public RssReadersJob()
        {
            var itanDatabaseContext = new ItanDatabaseContext();
            var repository = new EntityRepository(itanDatabaseContext, null);
            //this.testService = new TestService(repository, itanDatabaseContext);
        }

        public void Execute()
        {
            //this.testService.ReadRandomRssEvent();
        }
    }
}