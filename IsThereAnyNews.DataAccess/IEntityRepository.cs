namespace IsThereAnyNews.DataAccess
{
    using System;
    using System.Collections.Generic;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.SharedData;

    public interface IEntityRepository
    {
        void AddCommentRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddEventRssSkipped(long cui, List<long> id);

        void AddEventRssViewed(long currentUserId, long rssToReadId);

        void AddFullArticleRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddNotReadRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddReadLaterRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddShareRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddVoteDownRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AddVoteUpRequestByUserForArticle(long userId, StreamType modelStreamType, long id);

        void AssignUserRole(long currentUserId);

        void Blah();

        bool CanRegisterWithinLimits();

        void ChangeApplicationRegistration(RegistrationSupported dtoStatus);

        void ChangeDisplayName(long currentUserId, string displayname);

        void ChangeEmail(long currentUserId, string modelEmail);

        void ChangeUserLimit(long dtoLimit);

        void CopyAllUnreadElementsToUser(long currentUserId);

        void CreateNewSociaLogin(string identifierValue, AuthenticationTypeProvider authenticationTypeProvider, long newUserId);

        void CreateNewSubscription(long followerId, long observedId);

        void CreateNewSubscriptionForUserAndChannel(long userId, long channelId);

        long CreateNewUser(string name, string email);

        void DeleteSubscriptionFromUser(long subscriptionId, long userId);

        void DeleteUserSubscription(long followerId, long observedId);

        bool DoesUserOwnsUserSubscription(long subscriptionId, long currentUserId);

        SocialLogin FindSocialLogin(string socialLoginId, AuthenticationTypeProvider provider);

        long FindSubscriptionIdOfUserAndOfChannel(long userId, long channelId);
        void MarkRssClicked(long id, long subscriptionId);
        void AddEventRssClicked(long cui, long id);
        List<User> GetAllUsers();

        List<long> GetChannelIdSubscriptionsForUser(long currentUserId);
        void MarkRssNavigated(long rssId, long subscriptionId);
        void AddEventRssNavigated(long userId, long rssId);
        RegistrationSupported GetCurrentRegistrationStatus();

        List<long> GetIdByChannelUrl(List<string> urlstoChannels);

        DateTime GetLatestUpdateDate(long rssChannelId);

        long GetNumberOfRegisteredUsers();

        long GetNumberOfRssNews();

        long GetNumberOfRssSubscriptions();

        List<UserRole> GetRolesForUser(long currentUserId);

        List<ItanRole> GetRolesTypesForUser(long currentUserId);

        long GetUserId(string currentUserSocialLoginId, AuthenticationTypeProvider currentUserLoginProvider);

        UserPrivateProfileDto GetUserPrivateDetails(long currentUserId);

        void InsertReadRssToRead(long userId, long rssId, long dtoSubscriptionId);

        bool IsUserSubscribedToChannelId(long currentUserId, long channelId);

        bool IsUserSubscribedToChannelUrl(long currentUserId, string rssChannelLink);

        bool IsUserSubscribedToUser(long followerId, long observedId);

        List<RssChannel> LoadAllChannels();

        List<RssChannel> LoadAllChannelsForUser(long userIdToLoad);

        List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics(long currentUserId, int skip, int take);

        List<RssEntryToReadDTO> LoadAllEntriesFromSubscription(long subscriptionId);

        List<RssChannelSubscriptionDTO> LoadAllSubscriptionsForUser(long currentUserId);

        List<RssChannelSubscription> LoadAllSubscriptionsWithRssEntriesToReadForUser(long currentUserId);

        List<RssEntryToReadDTO> LoadAllUnreadEntriesFromSubscription(long subscriptionId);

        List<UserSubscriptionEntryToReadDTO> LoadAllUserEntriesFromSubscription(long subscriptionId);

        List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount();

        List<UserSubscriptionEntryToReadDTO> LoadAllUserUnreadEntriesFromSubscription(long subscriptionId);

        ApplicationConfigurationDTO LoadApplicationConfiguration();

        RssChannelInformationDTO LoadChannelChannelInformation(long subscriptionId);

        RssChannelForUpdateDTO LoadChannelToUpdate();

        RssChannelExceptions LoadExceptionEventsCount(long currentUserId);

        List<ExceptionEventDto> LoadExceptionList(long userId);

        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);

        List<RssEntryDTO> LoadRss(long subscriptionId, long userId);

        RssChannelDTO LoadRssChannel(long id);

        List<ChannelUpdateEventDto> LoadUpdateEvents(long userId);
        List<ChannelCreateEventDto> LoadCreationEvents(long userId);

        ChannelUrlAndTitleDTO LoadUrlAndTitle(long channelId);

        List<string> LoadUrlsForAllChannels();

        RssChannelInformationDTO LoadUserChannelInformation(long subscriptionId);

        UserPublicProfileDto LoadUserPublicProfile(long id);

        void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> rssId);

        void MarkChannelRead(List<long> ids);

        void MarkPersonEntriesSkipped(long modelSubscriptionId, List<long> ids);

        void MarkRssEntriesSkipped(long subscriptionId, List<long> ids);

        void SaveChannelCreatedEventToDatabase(long eventRssChannelCreated);

        void SaveContactAdministrationEventEventToDatabase(long contactId);

        long SaveContactAdministrationToDatabase(ContactAdministrationDto dto);

        void SaveEvent(long eventRssChannelUpdated);

        void SaveToDatabase(List<RssSourceWithUrlAndTitle> channelsNewToGlobalSpace);
        void SaveToDatabase(List<NewRssEntryDTO> rssEntriesList);
        void SaveToDatabase(SocialLogin socialLogin);
        void SaveToDatabase(List<RssChannelSubscription> rssChannelSubscriptions);

        void SaveToDatabase(IEnumerable<ItanException> exceptions);

        long SaveToDatabase(ContactAdministrationDto entity);

        void SaveExceptionToDatabase(IEnumerable<EventItanException> events);

        void Subscribe(long idByChannelUrl, long currentUserId, string channelIdRssChannelName);

        void Subscribe(long idByChannelUrl, long currentUserId);

        void UpdateDisplayNames(List<User> emptyDisplay);

        void UpdateRssLastUpdateTimeToDatabase(List<long> rssChannels);

        void UpdateUserLastReadTime(long currentUserId, DateTime now);

        bool UserIsRegistered(AuthenticationTypeProvider authenticationTypeProvider, string userId);
        void MarkPersonActivityClicked(long id, long subscriptionId);
        void AddEventPersonActivityClicked(long cui, long id);
        void MarkPersonActivityNavigated(long userId, long rssId);
        void AddEventPersonActivityNavigated(long userId, long rssId);
        void MarkPersonActivitySkipped(long subscriptionId, List<long> entries);
        void AddEventPersonActivitySkipped(long cui, List<long> entries);
        RssChannelCreations LoadCreateEventsCount(long currentUserId);
        void MarkExceptionActivityClicked(long cui, long id);
        void MarkExceptionActivitySkipped(long cui, List<long> entries);
        RssChannelUpdateds LoadUpdateEventsCount(long currentUserId);
        void MarkChannelUpdateClicked(long cui, long id);
        void MarkChannelUpdateSkipped(long cui, List<long> entries);
        void MarkChannelCreateClicked(long cui, long id);
        void MarkChannelCreateSkipped(long cui, List<long> entries);
        FeedEntries GetFeedEntries(long feedId, long skip, long take);
        NumberOfRssFeeds ReadNumberOfAllRssFeeds();
    }
}