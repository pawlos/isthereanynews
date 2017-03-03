namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using EntityFramework.Models.Entities;

    using Implementation;

    using IsThereAnyNews.ProjectionModels;

    public interface IUserRepository
    {
        long CreateNewUser(string name, string email);

        List<User> GetAllUsers();
        void UpdateDisplayNames(List<User> emptyDisplay);
        List<Dtos.UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount();
        UserPublicProfileDto LoadUserPublicProfile(long id);
        UserPrivateProfileDto GetUserPrivateDetails(long currentUserId);
        void ChangeEmail(long currentUserId, string email);
        void ChangeDisplayName(long currentUserId, string displayname);
    }
}