using System.Linq;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models;

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