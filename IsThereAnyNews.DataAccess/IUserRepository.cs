namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using EntityFramework.Models.Entities;

    using Implementation;

    public interface IUserRepository
    {
        User CreateNewUser();

        List<User> GetAllUsers();
        void UpdateDisplayNames(List<User> emptyDisplay);
        List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount();
        User LoadUserPublicProfile(long id);
        User GetUserPrivateDetails(long currentUserId);
    }
}