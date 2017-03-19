namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.Services;

    public class RssUpdateJob : IJob
    {
        private Service service;

        public RssUpdateJob()
        {
            var configureMapper = IsThereAnyNewsAutomapper.ConfigureMapper();

            var itanDatabaseContext = new ItanDatabaseContext();
            var updateRepository = new EntityRepository(itanDatabaseContext, null);

            this.service = new Service(updateRepository, configureMapper, null);

        }

        public void Execute()
        {
            this.service.UpdateGlobalRss();
        }
    }
}
