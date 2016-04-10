using System;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;

    public class UserRepository : IUserRepository
    {
        private readonly ItanDatabaseContext database;

        public UserRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public User CreateNewUser()
        {
            var user = new User
            {
                DisplayName = Faker.Name.FullName(),
                Email = Faker.Internet.Email()
            };

            this.database.Users.Add(user);
            this.database.SaveChanges();
            return user;
        }

        public void UpdateUserLastReadTime(long userId, DateTime now)
        {
            var single = this.database.Users.Single(user => user.Id == userId);
            single.LastReadTime = now;
            this.database.SaveChanges();
        }

        public DateTime GetUserLastReadTime(long userId)
        {
            var single = this.database.Users.Single(user => user.Id == userId);
            return single.LastReadTime;
        }

        public List<User> GetAllUsers()
        {
            return this.database.Users.ToList();
        }

        public void UpdateDisplayNames(List<User> emptyDisplay)
        {
            var ids = emptyDisplay.Select(x => x.Id).ToList();
            var users = this.database.Users.Where(user => ids.Contains(user.Id)).ToList();
            emptyDisplay.ForEach(newname => users.Single(u => u.Id == newname.Id).DisplayName = newname.DisplayName);
            this.database.SaveChanges();
        }
    }
}