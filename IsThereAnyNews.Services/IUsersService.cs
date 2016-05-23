using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IUsersService
    {
        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();
        UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id);
        void SubscribeToUser(long viewingUserId);
    }
}