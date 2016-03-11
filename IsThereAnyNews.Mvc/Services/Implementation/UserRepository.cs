using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ItanDatabaseContext context;

        public UserRepository() : this(new ItanDatabaseContext())
        {
        }

        public UserRepository(ItanDatabaseContext context)
        {
            this.context = context;
        }

        public void AddToUserRssList(string currentUserId, List<RssChannel> importFromUpload)
        {
            var singleAsync = this.context.Users
                .Single(user => user.Id == currentUserId);
            singleAsync.RssChannels.AddRange(importFromUpload);
            this.context.SaveChanges();
        }
    }
}