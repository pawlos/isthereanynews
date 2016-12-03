namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
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
                this.database.RssChannels.Include(x => x.Updates)
                    .Select(
                        x =>
                            new RssChannelForUpdateDTO
                            {
                                Id = x.Id,
                                RssLastUpdatedTime = x.Updates
                                                      .Select(s => s.Created)
                                                      .OrderByDescending(o => o)
                                                      .FirstOrDefault(),
                                Url = x.Url
                            })
                    .ToList();

            return rssChannels;
        }
    }
}