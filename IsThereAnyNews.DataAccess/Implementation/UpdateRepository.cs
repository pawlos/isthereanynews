namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Data.Entity;
    using System.Linq;

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

        public RssChannelForUpdateDTO LoadChannelToUpdate()
        {
            var sqlQuery = @"select top 1 c.Id,c.Url,
case when(u.c is null)then convert(datetime2, '0001-01-01 00:00:00.0000000', 121) else u.c end as Updated
from RssChannels c
left join 
(select u.RssChannelId, max(u.Created) c from EventRssChannelUpdates u
group by u.RssChannelId) u
on c.Id=u.RssChannelId
order by Updated";

            var rssChannelForUpdateDto = this.database.Database.SqlQuery<RssChannelForUpdateDTO>(sqlQuery);

            return rssChannelForUpdateDto.Single();
        }
    }
}