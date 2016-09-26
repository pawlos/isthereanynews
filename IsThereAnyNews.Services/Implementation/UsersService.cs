namespace IsThereAnyNews.Services.Implementation
{
    using System.Linq;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class UsersService : IUsersService
    {
        private readonly IUserRepository usersRepository;
        private readonly ISessionProvider sessionProvider;
        private readonly IUsersSubscriptionRepository usersSubscriptionRepository;

        public UsersService(IUserRepository usersRepository,
                            ISessionProvider sessionProvider,
                            IUsersSubscriptionRepository usersSubscriptionRepository)
        {
            this.usersRepository = usersRepository;
            this.sessionProvider = sessionProvider;
            this.usersSubscriptionRepository = usersSubscriptionRepository;
        }

        public AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile()
        {
            var publicProfiles = this.usersRepository.LoadAllUsersPublicProfileWithChannelsCount();
            var list = publicProfiles.Select(this.ProjectToViewModel).ToList();
            var viewmodel = new AllUsersPublicProfilesViewModel
            {
                Profiles = list
            };
            return viewmodel;
        }

        public UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id)
        {
            var cui = this.sessionProvider.GetCurrentUserId();
            var isUserAlreadySubscribed = this.usersSubscriptionRepository.IsUserSubscribedToUser(cui, id);
            var publicProfile = this.usersRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel = new UserDetailedPublicProfileViewModel
            {
                DisplayName = publicProfile.DisplayName,
                ChannelsCount = publicProfile.RssSubscriptionList.Count,
                Channels = publicProfile.RssSubscriptionList.Distinct().Select(channelSubscription => new PublicProfileChannelInformation
                {
                    Id = channelSubscription.Id,
                    Name = channelSubscription.Title
                }).ToList(),
                Events = publicProfile.EventsRssViewed.Select(e => new EventRssViewedViewModel
                {
                    Title = e.RssEntry.Title,
                    Viewed = e.Created,
                    RssId = e.RssEntryId
                }).ToList(),
                EventsCount = publicProfile.EventsRssViewed.Count,
                ViewingUserId = id,
                IsUserAlreadySubscribed = isUserAlreadySubscribed
            };
            return userDetailedPublicProfileViewModel;
        }

        public void UnsubscribeToUser(SubscribeToUserActivityDto model)
        {
            var cui = this.sessionProvider.GetCurrentUserId();
            this.usersSubscriptionRepository.DeleteUserSubscription(cui, model.ViewingUserId);
        }

        public void SubscribeToUser(SubscribeToUserActivityDto model)
        {
            var currentUserId = this.sessionProvider.GetCurrentUserId();
            this.usersSubscriptionRepository.CreateNewSubscription(currentUserId, model.ViewingUserId);
        }

        private UserPublicProfileViewModel ProjectToViewModel(UserPublicProfile model)
        {
            var viewModel = new UserPublicProfileViewModel
            {
                Id = model.Id,
                DisplayName = model.DisplayName,
                ChannelsCount = model.ChannelsCount
            };
            return viewModel;
        }
    }
}