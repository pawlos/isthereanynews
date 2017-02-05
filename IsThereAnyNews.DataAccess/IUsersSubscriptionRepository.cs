namespace IsThereAnyNews.DataAccess
{
    using System;
    using System.Collections.Generic;

    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public interface IUsersSubscriptionRepository
    {
        void CreateNewSubscription(long followerId, long observedId);
        void DeleteUserSubscription(long followerId, long observedId);
        bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId);
        bool IsUserSubscribedToUser(long followerId, long observedId);
        List<UserSubscriptionEntryToReadDTO> LoadAllEntriesFromSubscription(long subscriptionId);
        List<UserSubscriptionEntryToReadDTO> LoadAllUnreadEntriesFromSubscription(long subscriptionId);
        RssChannelInformationDTO LoadChannelInformation(long subscriptionId);
        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);
        void UpdateUserLastReadTime(long currentUserId, DateTime now);
        void MarkEntriesSkipped(long modelSubscriptionId, List<long> ids);
    }
}