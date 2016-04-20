using System;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    public interface IUserRepository
    {
        User CreateNewUser();
        void UpdateUserLastReadTime(long userId, DateTime now);
        DateTime GetUserLastReadTime(long userId);

        List<User> GetAllUsers();
        void UpdateDisplayNames(List<User> emptyDisplay);
        List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount();
        User LoadUserPublicProfile(long id);
    }
}