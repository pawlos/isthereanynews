namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class UpdateRepository : IUpdateRepository
    {
        private readonly ItanDatabaseContext database;

        public UpdateRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<RssChannel> LoadAllGlobalRssChannelsSortedByUpdate()
        {
            var rssChannels = this.database.RssChannels.Include(x => x.Updates).ToList();
            return rssChannels;
        }
    }
}