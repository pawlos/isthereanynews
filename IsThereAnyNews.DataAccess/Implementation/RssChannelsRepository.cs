using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelsRepository : IRssChannelsRepository
    {
        private readonly ItanDatabaseContext database;

        public RssChannelsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<RssChannel> AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            var rssChannels = importFromUpload.Select(channel => new RssChannel(channel.Url, channel.Title)).ToList();
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
            return rssChannels;
        }

        public List<RssChannel> LoadAllChannels()
        {
            return this.database.RssChannels.ToList();
        }

        public RssChannel Load(long id)
        {
            return this.database.RssChannels.Single(channel => channel.Id == id);
        }

        public List<RssChannel> LoadAllChannelsForUser(long userIdToLoad)
        {
            var rssChannels = this.database
                .Users
                .Where(user => user.Id == userIdToLoad)
                .Include(user => user.RssSubscriptionList)
                .Include(user => user.RssSubscriptionList.Select(rsl => rsl.RssChannel))
                .Single()
                .RssSubscriptionList.Select(rsl => rsl.RssChannel)
                .ToList();
            return rssChannels;
        }

        public void SaveToDatabase(List<RssChannel> channelsNewToGlobalSpace)
        {
            this.database.RssChannels.AddRange(channelsNewToGlobalSpace);
            this.database.SaveChanges();
        }

        public List<long> GetIdByChannelUrl(List<string> urlstoChannels)
        {
            var longs = this.database.RssChannels.
                Where(channel => urlstoChannels.Contains(channel.Url))
                .Select(channel => channel.Id)
                .ToList();

            return longs;
        }

    }
}