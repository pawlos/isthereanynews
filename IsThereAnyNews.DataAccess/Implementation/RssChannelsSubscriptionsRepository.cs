using System.Collections.Generic;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssChannelsSubscriptionsRepository : IRssChannelsSubscriptionsRepository
    {
        private readonly ItanDatabaseContext itanDatabaseContext;

        public RssChannelsSubscriptionsRepository() : this(new ItanDatabaseContext())
        {

        }

        private RssChannelsSubscriptionsRepository(ItanDatabaseContext itanDatabaseContext)
        {
            this.itanDatabaseContext = itanDatabaseContext;
        }

        public void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions)
        {
            this.itanDatabaseContext.RssChannelsSubscriptions.AddRange(rssChannelSubscriptions);
            this.itanDatabaseContext.SaveChanges();
        }
    }
}