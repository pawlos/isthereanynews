namespace IsThereAnyNews.Services.Implementation
{
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.DataAccess.Implementation;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ViewModels;

    public class UsersService : IUsersService
    {
        private readonly IUserRepository usersRepository;

        private readonly IUsersSubscriptionRepository usersSubscriptionRepository;

        private readonly IUserAuthentication authentication;

        private readonly IMapper mapper;

        public UsersService(IUserRepository usersRepository,
                            IUsersSubscriptionRepository usersSubscriptionRepository,
                            IUserAuthentication authentication, 
                            IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.usersSubscriptionRepository = usersSubscriptionRepository;
            this.authentication = authentication;
            this.mapper = mapper;
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
            var cui = this.authentication.GetCurrentUserId();
            var isUserAlreadySubscribed = this.usersSubscriptionRepository.IsUserSubscribedToUser(cui, id);
            var publicProfile = this.usersRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel = this.mapper.Map<User, UserDetailedPublicProfileViewModel>(publicProfile);
            userDetailedPublicProfileViewModel.IsUserAlreadySubscribed = isUserAlreadySubscribed;

            userDetailedPublicProfileViewModel.Events =
                userDetailedPublicProfileViewModel.Events.OrderByDescending(e => e.Viewed).ToList();

            return userDetailedPublicProfileViewModel;
        }

        public void UnsubscribeToUser(SubscribeToUserActivityDto model)
        {
            var cui = this.authentication.GetCurrentUserId();
            this.usersSubscriptionRepository.DeleteUserSubscription(cui, model.ViewingUserId);
        }

        public void SubscribeToUser(SubscribeToUserActivityDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
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