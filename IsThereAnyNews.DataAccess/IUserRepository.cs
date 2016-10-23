namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using EntityFramework.Models.Entities;

    using Implementation;

    public interface IUserRepository
    {
        User CreateNewUser(string name, string email);

        List<User> GetAllUsers();
        void UpdateDisplayNames(List<User> emptyDisplay);
        List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount();
        User LoadUserPublicProfile(long id);
        User GetUserPrivateDetails(long currentUserId);
        void ChangeEmail(long currentUserId, string email);
        void ChangeDisplayName(long currentUserId, string displayname);
    }
}