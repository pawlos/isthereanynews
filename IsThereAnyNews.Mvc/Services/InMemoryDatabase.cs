using System.Collections.Generic;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models;

    public class InMemoryDatabase : IItanDatabase
    {
        private static List<ApplicationUser> applicationUsers = new List<ApplicationUser>();

        public List<ApplicationUser> ApplicationUsers => applicationUsers;
        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }

}