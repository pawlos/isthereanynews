using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Collections.Generic;

    public interface IUserRepository
    {
        void AddToUserRssList(string currentUserId, List<RssChannel> importFromUpload);
    }
}