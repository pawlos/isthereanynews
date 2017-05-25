using System.Security.Claims;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;
using IsThereAnyNews.ViewModels.RssChannel;

namespace IsThereAnyNews.Web.Interfaces.Services
{
    using IsThereAnyNews.Dtos.Feeds;

    public interface IService
    {
        AccountDetailsViewModel GetAccountDetailsViewModel();
        void ChangeDisplayName(ChangeDisplayNameModelDto model);
        void ChangeEmail(ChangeEmailModelDto model);
        void ChangeRegistration(ChangeRegistrationDto dto);
        void ChangeUsersLimit(ChangeUsersLimitDto dto);
        ItanApplicationConfigurationViewModel ReadConfiguration();
        void UpdateGlobalRss();
        ContactViewModel GetViewModel();
        void MarkClicked(EntryClickedDto dto);
        void MarkNavigated(EntryNavigatedDto dto);
        void AddCommentToRssItemByCurrentUser(RssActionModel model);
        void AddToReadLaterQueueForCurrentUser(RssActionModel model);
        void MarkRssItemAsNotReadByCurrentUser(RssActionModel model);
        void ShareRssItem(RssActionModel model);
        void CurrentVotedownForArticleByCurrentUser(RssActionModel model);
        void CurrentVoteupForArticleByCurrentUser(RssActionModel model);
        RssChannelsIndexViewModel LoadPublicRssFeeds(FeedsGetPublic input);
        FeedEntriesViewModel GetFeedEntries(FeedsGetEntries id);
        void UnsubscribeCurrentUserFromChannelId(FeedsPostSubscription model);
        void CreateNewChannelIfNotExists(AddChannelDto dto);
        void StoreCurrentUserIdInSession(ClaimsIdentity identity);
        void StoreItanRolesToSession(ClaimsIdentity identity);
        void Import(OpmlImporterIndexDto dto);
        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();
        UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id);
        void SubscribeToUser(SubscribeToUserActivityDto model);
        void UnsubscribeToUser(SubscribeToUserActivityDto model);
        RssChannelsMyViewModel LoadAllChannelsOfCurrentUser();
        void MarkAllRssReadForSubscription(MarkReadForSubscriptionDto model);
        void SaveAdministrationContact(ContactAdministrationDto dto);
        void MarkEntriesSkipped(EntriesSkippedDto model);
        RegistrationSupported GetCurrentRegistrationStatus();
        bool CanRegisterIfWithinLimits();
        bool IsUserRegistered(ClaimsIdentity identity);
        void RegisterIfNewUser(ClaimsIdentity identity);
        void SubscribeCurrentUserToChannel(FeedsPostSubscription model);
        void SubscribeCurrentUserToChannel(AddChannelDto channelId);
        ISubscriptionContentIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(FeedsGetRead input);
        FeedsIndexViewModel LoadPublicFeedsNumbers();
    }
}