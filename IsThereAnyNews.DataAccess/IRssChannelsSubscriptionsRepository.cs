using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelsSubscriptionsRepository
    {
        void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions);
        List<string> LoadUrlsForAllChannels();
        List<long> GetChannelIdSubscriptionsForUser(long currentUserId);
        List<RssChannelSubscription> LoadAllSubscriptionsWithChannelsForUser(long currentUserId);
    }
}