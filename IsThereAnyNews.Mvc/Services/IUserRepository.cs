namespace IsThereAnyNews.Mvc.Services
{
    using System.Collections.Generic;
    using Models;

    public interface IUserRepository
    {
        void AddToUserRssList(string currentUserId, List<RssChannel> importFromUpload);
    }
}