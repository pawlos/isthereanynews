using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
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

        public List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics()
        {
            return this.database
                .RssChannels
                .Include(r => r.Subscriptions)
                .Include(r => r.RssEntries)
                .Select(ProjectToRssChannelSubscriptionWithStatisticsData)
                .ToList();
        }

        private RssChannelSubscriptionWithStatisticsData ProjectToRssChannelSubscriptionWithStatisticsData(RssChannel model)
        {
            return new RssChannelSubscriptionWithStatisticsData(
                model.Id,
                model.Title,
                model.Subscriptions.Count(),
                model.RssEntries.Count(),
                model.Created,
                model.Updated);
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

        public RssChannel LoadRssChannel(long id)
        {
            return this.database
                .RssChannels
                .Include(channel => channel.RssEntries)
                .Single(x => x.Id == id);
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

        public void Blah()
        {
            var x = new[]
            {
                "http://feeds.reuters.com/news/artsculture",
                "http://feeds.reuters.com/reuters/businessNews",
                "http://feeds.reuters.com/reuters/companyNews",
                "http://feeds.reuters.com/reuters/entertainment",
                "http://feeds.reuters.com/reuters/environment",
                "http://feeds.reuters.com/reuters/healthNews",
                "http://feeds.reuters.com/reuters/lifestyle",
                "http://feeds.reuters.com/news/reutersmedia",
                "http://feeds.reuters.com/news/wealth",
                "http://feeds.reuters.com/reuters/MostRead",
                "http://feeds.reuters.com/reuters/oddlyEnoughNews",
                "http://feeds.reuters.com/ReutersPictures",
                "http://feeds.reuters.com/reuters/peopleNews",
                "http://feeds.reuters.com/Reuters/PoliticsNews",
                "http://feeds.reuters.com/reuters/scienceNews",
                "http://feeds.reuters.com/reuters/sportsNews",
                "http://feeds.reuters.com/reuters/technologyNews",
                "http://feeds.reuters.com/reuters/topNews",
                "http://feeds.reuters.com/Reuters/domesticNews",
                "http://feeds.reuters.com/Reuters/worldNews",
                "http://feeds.reuters.com/reuters/bankruptcyNews",
                "http://feeds.reuters.com/reuters/bondsNews",
                "http://feeds.reuters.com/news/deals",
                "http://feeds.reuters.com/news/economy",
                "http://feeds.reuters.com/reuters/globalmarketsNews",
                "http://feeds.reuters.com/news/hedgefunds",
                "http://feeds.reuters.com/reuters/hotStocksNews",
                "http://feeds.reuters.com/reuters/mergersNews",
                "http://feeds.reuters.com/reuters/governmentfilingsNews",
                "http://feeds.reuters.com/reuters/summitNews",
                "http://feeds.reuters.com/reuters/USdollarreportNews",
                "http://feeds.reuters.com/news/usmarkets",
                "http://feeds.reuters.com/reuters/basicmaterialsNews",
                "http://feeds.reuters.com/reuters/cyclicalconsumergoodsNews",
                "http://feeds.reuters.com/reuters/USenergyNews",
                "http://feeds.reuters.com/reuters/environment",
                "http://feeds.reuters.com/reuters/financialsNews",
                "http://feeds.reuters.com/reuters/UShealthcareNews",
                "http://feeds.reuters.com/reuters/industrialsNews",
                "http://feeds.reuters.com/reuters/USmediaDiversifiedNews",
                "http://feeds.reuters.com/reuters/noncyclicalconsumergoodsNews",
                "http://feeds.reuters.com/reuters/technologysectorNews",
                "http://feeds.reuters.com/reuters/utilitiesNews",
                "http://feeds.reuters.com/reuters/blogs/FinancialRegulatoryForum",
                "http://feeds.reuters.com/reuters/blogs/GlobalInvesting",
                "http://feeds.reuters.com/reuters/blogs/HugoDixon",
                "http://feeds.reuters.com/reuters/blogs/India",
                "http://feeds.reuters.com/reuters/blogs/JamesSaft",
                "http://feeds.reuters.com/reuters/blogs/macroscope",
                "http://feeds.reuters.com/reuters/blogs/mediafile",
                "http://feeds.reuters.com/reuters/blogs/newsmaker",
                "http://feeds.reuters.com/reuters/blogs/photo",
                "http://feeds.reuters.com/reuters/blogs/SummitNotebook",
                "http://feeds.reuters.com/reuters/blogs/talesfromthetrail",
                "http://feeds.reuters.com/reuters/blogs/the-great-debate",
                "http://feeds.reuters.com/UnstructuredFinance",
                "http://feeds.reuters.com/reuters/USVideoBreakingviews",
                "http://feeds.reuters.com/reuters/USVideoBusiness",
                "http://feeds.reuters.com/reuters/USVideoBusinessTravel",
                "http://feeds.reuters.com/reuters/USVideoChrystiaFreeland",
                "http://feeds.reuters.com/reuters/USVideoEntertainment",
                "http://feeds.reuters.com/reuters/USVideoEnvironment",
                "http://feeds.reuters.com/reuters/USVideoFelixSalmon",
                "http://feeds.reuters.com/reuters/USVideoGigaom",
                "http://feeds.reuters.com/reuters/USVideoLifestyle",
                "http://feeds.reuters.com/reuters/USVideoMostWatched",
                "http://feeds.reuters.com/reuters/USVideoLatest",
                "http://feeds.reuters.com/reuters/USVideoNewsmakers",
                "http://feeds.reuters.com/reuters/USVideoOddlyEnough",
                "http://feeds.reuters.com/reuters/USVideoPersonalFinance",
                "http://feeds.reuters.com/reuters/USVideoPolitics",
                "http://feeds.reuters.com/reuters/USVideoRoughCuts",
                "http://feeds.reuters.com/reuters/USVideoSmallBusiness",
                "http://feeds.reuters.com/reuters/USVideoTechnology",
                "http://feeds.reuters.com/reuters/USVideoTopNews",
                "http://feeds.reuters.com/reuters/USVideoWorldNews",
                "http://rss.cnn.com/rss/edition.rss",
                "http://rss.cnn.com/rss/edition_world.rss",
                "http://rss.cnn.com/rss/edition_africa.rss",
                "http://rss.cnn.com/rss/edition_americas.rss",
                "http://rss.cnn.com/rss/edition_asia.rss",
                "http://rss.cnn.com/rss/edition_europe.rss",
                "http://rss.cnn.com/rss/edition_meast.rss",
                "http://rss.cnn.com/rss/edition_us.rss",
                "http://rss.cnn.com/rss/money_news_international.rss",
                "http://rss.cnn.com/rss/edition_technology.rss",
                "http://rss.cnn.com/rss/edition_space.rss",
                "http://rss.cnn.com/rss/edition_entertainment.rss",
                "http://rss.cnn.com/rss/edition_sport.rss",
                "http://rss.cnn.com/rss/edition_football.rss",
                "http://rss.cnn.com/rss/edition_golf.rss",
                "http://rss.cnn.com/rss/edition_motorsport.rss",
                "http://rss.cnn.com/rss/edition_tennis.rss",
                "http://rss.cnn.com/rss/edition_travel.rss",
                "http://rss.cnn.com/rss/cnn_freevideo.rss",
                "http://rss.cnn.com/rss/cnn_latest.rss",
            };

            var rssChannels = x.Select(b => new RssChannel(b, b)).ToList();
            this.database.RssChannels.AddRange(rssChannels);
            this.database.SaveChanges();
        }

        public List<RssChannel> LoadAllChannels()
        {
            return this.database.RssChannels.ToList();
        }
    }

    public class RssChannelSubscriptionWithStatisticsData
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int SubscriptionsCount { get; set; }
        public int RssEntriesCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public RssChannelSubscriptionWithStatisticsData(
            long id,
            string title,
            int subscriptionsCount,
            int i,
            DateTime created, DateTime updated)
        {
            Id = id;
            Title = title;
            SubscriptionsCount = subscriptionsCount;
            RssEntriesCount = i;
            Created = created;
            Updated = updated;
        }
    }
}