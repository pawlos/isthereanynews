namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public interface IUsersService
    {
        AllUsersPublicProfilesViewModel LoadAllUsersPublicProfile();

        UserDetailedPublicProfileViewModel LoadUserPublicProfile(long id);

        void SubscribeToUser(SubscribeToUserActivityDto model);

        void UnsubscribeToUser(SubscribeToUserActivityDto model);
    }
}