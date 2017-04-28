namespace IsThereAnyNews.RssChannelUpdater
{
    using FluentScheduler;

    using IsThereAnyNews.Automapper;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.Infrastructure.Import.Opml;
    using IsThereAnyNews.Infrastructure.Web;
    using IsThereAnyNews.Services;

    public class RssUpdateJob : IJob
    {
        private Service service;

        public RssUpdateJob()
        {
            var configureMapper = IsThereAnyNewsAutomapper.ConfigureMapper();

            var itanDatabaseContext = new ItanDatabaseContext();
            var updateRepository = new EntityRepository(itanDatabaseContext,configureMapper);
            var infrastructure = new Infrastructure();
            var importOpml = new ImportOpml();

            this.service = new Service(updateRepository, configureMapper, null, infrastructure, importOpml);

        }

        public void Execute()
        {
            this.service.UpdateGlobalRss();
        }
    }
}
