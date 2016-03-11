using IsThereAnyNews.Mvc.Controllers;
using IsThereAnyNews.Mvc.Services;
using IsThereAnyNews.Mvc.Services.Implementation;

namespace IsThereAnyNews.Mvc.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class RssChannelRepository : IRssChannelRepository
    {
        private readonly IItanDatabase database;

        public RssChannelRepository() : this(new InMemoryDatabase())
        {
        }

        public RssChannelRepository(IItanDatabase database)
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
                    Id = CreateId()
                };

                database.RssChannels.Add(rssChannel);
            }

            return savedList;
        }

        public List<RssChannel> LoadAllChannels()
        {
            return database.RssChannels;
        }

        public RssChannel Load(long id)
        {
            return database.RssChannels.Single(channel => channel.Id == id);
        }

        public List<RssChannel> LoadAllChannelsForUser(string currentUserId)
        {
            var rssChannels = this.database.ApplicationUsers
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