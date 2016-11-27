namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper.QueryableExtensions;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public class UpdateRepository : IUpdateRepository
    {
        private readonly ItanDatabaseContext database;

        public UpdateRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<RssChannelForUpdateDTO> LoadAllGlobalRssChannelsSortedByUpdate()
        {
            var rssChannels = 
                this
                .database
                .RssChannels
                .Include(x => x.Updates)
                .ProjectTo<RssChannelForUpdateDTO>()
                .ToList();
            return rssChannels;
        }
    }
}