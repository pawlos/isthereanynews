namespace IsThereAnyNews.Web.Interfaces.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Xml;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels.Mess;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.RssChannel;

    public interface IService
    {
        void AddCommentToRssItemByCurrentUser(RssActionModel id);

        void AddNewChannelsToGlobalSpace(List<RssSourceWithUrlAndTitle> channelList);

        void AddToReadLaterQueueForCurrentUser(RssActionModel id);

        bool CanRegisterIfWithinLimits();

        void ChangeDisplayName(ChangeDisplayNameModelDto model);

        void ChangeEmail(ChangeEmailModelDto model);

        void ChangeRegistration(ChangeRegistrationDto dto);

        void ChangeUsersLimit(ChangeUsersLimitDto dto);

        void CreateNewChannelIfNotExists(AddChannelDto dto);

        bool CurrentUserIsAuthenticated();

        void CurrentVotedownForArticleByCurrentUser(RssActionModel id);

        void CurrentVoteupForArticleByCurrentUser(RssActionModel id);

        AccountDetailsViewModel GetAccountDetailsViewModel();

        RegistrationSupported GetCurrentRegistrationStatus();

        ClaimsPrincipal GetCurrentUser();

        long GetCurrentUserId();

        AuthenticationTypeProvider GetCurrentUserLoginProvider(ClaimsIdentity identity);

        List<ItanRole> GetCurrentUserRoles();

        IEnumerable<XmlNode> GetOutlines(Stream inputStream);

        string GetUserSocialIdFromIdentity(ClaimsIdentity identity);

        ContactViewModel GetViewModel();

        RssChannelIndexViewModel GetViewModelFormChannelId(long id);

        void Import(OpmlImporterIndexDto dto);

        bool IsUserRegistered(ClaimsIdentity identity);

        List<SyndicationItemAdapter> Load(string url);

        RssChannelsIndexViewModel LoadAllChannels();

        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();

        List<ObservableUserEventsInformation> LoadAllObservableSubscription();

        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long id, ShowReadEntries showReadEntries);

        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();

        AdminEventsViewModel LoadEvents();

        UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id);

        void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto id);

        void MarkClicked(MarkClickedDto dto);

        void MarkEntriesRead(MarkReadDto dto);

        void MarkEntriesSkipped(MarkSkippedDto model);

        void MarkRead(MarkReadDto dto);

        void MarkRssItemAsNotReadByCurrentUser(RssActionModel id);

        List<RssSourceWithUrlAndTitle> ParseToRssChannelList(OpmlImporterIndexDto dto);

        ItanApplicationConfigurationViewModel ReadConfiguration();

        void RegisterIfNewUser(ClaimsIdentity identity);

        void SaveAdministrationContact(ContactAdministrationDto dto);

        void ShareRssItem(RssActionModel id);

        void StoreCurrentUserIdInSession(ClaimsIdentity identity);

        void StoreItanRolesToSession(ClaimsIdentity identity);

        void SubscribeCurrentUserToChannel(long channelId);

        void SubscribeCurrentUserToChannel(AddChannelDto channelId);

        void SubscribeToUser(SubscribeToUserActivityDto model);

        void UnsubscribeCurrentUserFromChannelId(long subscriptionId);

        void UnsubscribeToUser(SubscribeToUserActivityDto model);

        void UpdateGlobalRss();
    }
}