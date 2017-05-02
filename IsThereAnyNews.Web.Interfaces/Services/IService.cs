using System.Security.Claims;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.SharedData;
using IsThereAnyNews.ViewModels;
using IsThereAnyNews.ViewModels.RssChannel;

namespace IsThereAnyNews.Web.Interfaces.Services
{
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
        void MarkClicked(MarkClickedDto dto);
        void MarkEntriesRead(MarkReadDto dto);
        void MarkRead(MarkReadDto dto);
        void AddCommentToRssItemByCurrentUser(RssActionModel model);
        void AddToReadLaterQueueForCurrentUser(RssActionModel model);
        void MarkRssItemAsNotReadByCurrentUser(RssActionModel model);
        void ShareRssItem(RssActionModel model);
        void CurrentVotedownForArticleByCurrentUser(RssActionModel model);
        void CurrentVoteupForArticleByCurrentUser(RssActionModel model);
        RssChannelsIndexViewModel LoadAllChannels();
        RssChannelIndexViewModel GetViewModelFormChannelId(long id);
        void UnsubscribeCurrentUserFromChannelId(long channelId);
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
        void MarkEntriesSkipped(MarkSkippedDto model);
        RegistrationSupported GetCurrentRegistrationStatus();
        bool CanRegisterIfWithinLimits();
        bool IsUserRegistered(ClaimsIdentity identity);
        void RegisterIfNewUser(ClaimsIdentity identity);
        void SubscribeCurrentUserToChannel(long channelId);
        void SubscribeCurrentUserToChannel(AddChannelDto channelId);
        RssSubscriptionIndexViewModel LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(StreamType streamType, long id, ShowReadEntries showReadEntries);
    }
}