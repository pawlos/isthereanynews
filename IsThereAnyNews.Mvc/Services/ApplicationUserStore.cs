using System.Linq;

namespace IsThereAnyNews.Mvc.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Models;

    public class ApplicationUserStore : IUserStore<ApplicationUser>
    {
        private readonly IItanDatabase database;

        public ApplicationUserStore() : this(new InMemoryDatabase())
        {
        }

        private ApplicationUserStore(IItanDatabase database)
        {
            this.database = database;
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(ApplicationUser user)
        {
            this.database.ApplicationUsers.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            this.database.ApplicationUsers.ToList().Remove(user);
            return Task.CompletedTask;
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Task.Run(() => this.database.ApplicationUsers.FirstOrDefault(x => x.Id == userId));
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Run(() => this.database.ApplicationUsers.FirstOrDefault(x => x.UserName == userName));
        }
    }
}