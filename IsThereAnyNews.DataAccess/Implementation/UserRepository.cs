using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ItanDatabaseContext database;

        public UserRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public User CreateNewUser()
        {
            var user = new User();
            this.database.Users.Add(user);
            this.database.SaveChanges();
            return user;
        }
    }
}