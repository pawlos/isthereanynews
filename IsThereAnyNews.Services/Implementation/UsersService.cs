namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ProjectionModels;
    using IsThereAnyNews.ViewModels;

    public class UsersService : IUsersService
    {
        private readonly IUserAuthentication authentication;

        private readonly IEntityRepository entityRepository;
        private readonly IMapper mapper;
        public UsersService(
        IUserAuthentication authentication,
                            IMapper mapper,
                            IEntityRepository entityRepository)
        {
            this.authentication = authentication;
            this.mapper = mapper;
            this.entityRepository = entityRepository;
        }

        public AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile()
        {
            var publicProfiles = this.entityRepository.LoadAllUsersPublicProfileWithChannelsCount();
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
            var isUserAlreadySubscribed = this.entityRepository.IsUserSubscribedToUser(cui, id);
            var publicProfile = this.entityRepository.LoadUserPublicProfile(id);
            var userDetailedPublicProfileViewModel = this.mapper.Map<UserPublicProfileDto, UserDetailedPublicProfileViewModel>(publicProfile);
            userDetailedPublicProfileViewModel.IsUserAlreadySubscribed = isUserAlreadySubscribed;
            userDetailedPublicProfileViewModel.Events = userDetailedPublicProfileViewModel.Events.OrderByDescending(e => e.Viewed).ToList();

            var loadNameAndCountForUser = this.entityRepository.LoadNameAndCountForUser(id);

            var publicProfileUsersInformations =
                this.mapper
                    .Map<List<NameAndCountUserSubscription>, List<PublicProfileChannelInformation>>(loadNameAndCountForUser);

            userDetailedPublicProfileViewModel.Users = publicProfileUsersInformations;
            return userDetailedPublicProfileViewModel;
        }

        public void SubscribeToUser(SubscribeToUserActivityDto model)
        {
            var currentUserId = this.authentication.GetCurrentUserId();
            this.entityRepository.CreateNewSubscription(currentUserId, model.ViewingUserId);
        }

        public void UnsubscribeToUser(SubscribeToUserActivityDto model)
        {
            var cui = this.authentication.GetCurrentUserId();
            this.entityRepository.DeleteUserSubscription(cui, model.ViewingUserId);
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