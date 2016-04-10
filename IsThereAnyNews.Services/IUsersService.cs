using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.Services
{
    public interface IUsersService
    {
        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();
        UserDetailedPublicProfileViewModel LoadUserPublicProfile(int id);
    }
}