using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.Services.Implementation
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository usersRepository;

        public UsersService(IUserRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile()
        {
            var publicProfiles = this.usersRepository.LoadAllUsersPublicProfileWithChannelsCount();
            var list = publicProfiles.Select(ProjectToViewModel).ToList();
            var viewmodel = new AllUsersPublicProfilesViewModel
            {
                Profiles = list
            };
            return viewmodel;
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

    public class AllUsersPublicProfilesViewModel
    {
        public List<UserPublicProfileViewModel> Profiles { get; set; }
    }

    public class UserPublicProfileViewModel
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
    }
}