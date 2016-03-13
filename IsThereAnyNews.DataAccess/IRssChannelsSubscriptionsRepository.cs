using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelsSubscriptionsRepository
    {
        void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions);
    }
}