namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public interface IRssChannelsSubscriptionsRepository
    {
        void CreateNewSubscriptionForUserAndChannel(long userId, long channelId);

        void DeleteSubscriptionFromUser(long subscriptionId, long userId);

        bool DoesUserOwnsUserSubscription(long subscriptionId, long currentUserId);

        long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId);

        List<long> GetChannelIdSubscriptionsForUser(long currentUserId);

        bool IsUserSubscribedToChannelId(long currentUserId, long channelId);

        bool IsUserSubscribedToChannelUrl(long currentUserId, string rssChannelLink);

        List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser(long currentUserId);

        List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId);

        RssChannelInformationDTO LoadUserChannelInformation(long subscriptionId);

        List<string> LoadUrlsForAllChannels();

        void MarkChannelRead(List<long> ids);

        void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions);
        void Subscribe(long idByChannelUrl, long currentUserId, string channelIdRssChannelName);
        void Subscribe(long idByChannelUrl, long currentUserId);

        RssChannelInformationDTO LoadChannelChannelInformation(long subscriptionId);
    }
}