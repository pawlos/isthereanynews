using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class UserRepository : IUserRepository
    {
        private IItanDatabase context;

        public UserRepository() : this(new InMemoryDatabase())
        {
        }

        public UserRepository(IItanDatabase context)
        {
            this.context = context;
        }

        public async void AddToUserRssList(string currentUserId, List<RssChannel> importFromUpload)
        {
            var singleAsync = this.context.ApplicationUsers
                .Single(user => user.Id == currentUserId);
            singleAsync.RssChannels.AddRange(importFromUpload);
            await this.context.SaveAsync();
        }
    }
}