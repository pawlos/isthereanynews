using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.ProjectionModels;

    public interface IRssChannelsSubscriptionsRepository
    {
        void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions);
        List<string> LoadUrlsForAllChannels();
        List<long> GetChannelIdSubscriptionsForUser(long currentUserId);
        List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser(long currentUserId);
        List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId);
        bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId);
        void DeleteSubscriptionFromUser(long subscriptionId, long userId);
        long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId);
        void CreateNewSubscriptionForUserAndChannel(long userId, long channelId);
        RssChannelSubscription LoadChannelInformation(long subscriptionId);
        void MarkRead(List<long> ids);
        bool IsUserSubscribedToChannelUrl(long currentUserId, string rssChannelLink);
    }
}