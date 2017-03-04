namespace IsThereAnyNews.DataAccess
{
    using System;
    using System.Collections.Generic;

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

        List<User> GetAllUsers();

        List<long> GetChannelIdSubscriptionsForUser(long currentUserId);

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

        List<RssChannelSubscriptionWithStatisticsData> LoadAllChannelsWithStatistics();

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

        RssChannelCreations LoadCreateEvents();

        RssChannelExceptions LoadExceptionEvents();

        List<ExceptionEventDto> LoadLatest(int eventCount);

        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);

        List<RssEntryDTO> LoadRss(long subscriptionId, ShowReadEntries showReadEntries);

        RssChannelDTO LoadRssChannel(long id);

        RssChannelUpdateds LoadUpdateEvents();

        List<ChannelUpdateEventDto> LoadUpdateEvents(int eventsCount);

        ChannelUrlAndTitleDTO LoadUrlAndTitle(long channelId);

        List<string> LoadUrlsForAllChannels();

        RssChannelInformationDTO LoadUserChannelInformation(long subscriptionId);

        UserPublicProfileDto LoadUserPublicProfile(long id);

        void MarkAllReadForUserAndSubscription(long subscriptionId, List<long> rssId);

        void MarkChannelRead(List<long> ids);

        void MarkClicked(long dtoId, long currentUserId);

        void MarkEntriesSkipped(long modelSubscriptionId, List<long> ids);

        void MarkEntryViewedByUser(long currentUserId, long rssToReadId);

        void MarkPersonEntriesSkipped(long modelSubscriptionId, List<long> ids);

        void MarkRead(List<long> ids);

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
    }
}