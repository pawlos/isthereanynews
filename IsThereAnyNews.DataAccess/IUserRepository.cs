using System;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IUserRepository
    {
        User CreateNewUser();
        void UpdateUserLastReadTime(long userId, DateTime now);
        DateTime GetUserLastReadTime(long userId);
    }
}