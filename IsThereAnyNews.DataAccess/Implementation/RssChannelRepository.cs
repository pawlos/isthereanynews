using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelRepository : IRssChannelRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelRepository() : this(new ItanDatabaseContext())
        {
        }

        public RssChannelRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<RssChannel> AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            var savedList = new List<RssChannel>();
            foreach (var channel in importFromUpload)
            {
                var rssChannel = new RssChannel(channel.Url, channel.Title)
                {
                    Id = this.CreateId()
                };

                this.database.RssChannels.Add(rssChannel);
            }

            return savedList;
        }

        public List<RssChannel> LoadAllChannels()
        {
            return this.database.RssChannels.ToList();
        }

        public RssChannel Load(long id)
        {
            return this.database.RssChannels.Single(channel => channel.Id == id);
        }

        public List<RssChannel> LoadAllChannelsForUser(string currentUserId)
        {
            var rssChannels = this.database.Users
                .Single(user => user.Id == currentUserId)
                .RssChannels.ToList();
            return rssChannels;
        }

        private long CreateId()
        {
            return this.database.RssChannels.Any() ?
                this.database.RssChannels.Max(x => x.Id) + 1 : 0;
        }
    }
}