using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.Services.Implementation
{
    public class InMemoryDatabase : IItanDatabase
    {
        private static List<ApplicationUser> applicationUsers = new List<ApplicationUser>();

        public List<ApplicationUser> ApplicationUsers => applicationUsers;
        public List<RssChannel> RssChannels => applicationUsers.SelectMany(u => u.RssChannels).ToList();

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }

}