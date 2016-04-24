using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.Services
{
    public interface IUsersService
    {
        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();
        UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id);
        void SubscribeToUser(long viewingUserId);
    }
}