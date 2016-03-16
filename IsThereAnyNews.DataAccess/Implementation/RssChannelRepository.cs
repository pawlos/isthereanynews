using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelRepository : IRssChannelRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public RssChannel LoadRssChannel(long id)
        {
            return this.database.RssChannels.Single(x => x.Id == id);
        }

        public void UpdateRssLastUpdateTimeToDatabase(List<RssChannel> rssChannels)
        {
            var ids = rssChannels.Select(x => x.Id).ToList();

            var channels = this.database.RssChannels.Where(channel => ids.Contains(channel.Id)).ToList();
            channels.ForEach(channel =>
            {
                channel.RssLastUpdatedTime = rssChannels
                    .Single(x => x.Id == channel.Id).RssLastUpdatedTime;
            });

            this.database.SaveChanges();
        }
    }
}