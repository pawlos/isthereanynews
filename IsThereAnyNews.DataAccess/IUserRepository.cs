using System;
using IsThereAnyNews.EntityFramework.Models;

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
    }
}