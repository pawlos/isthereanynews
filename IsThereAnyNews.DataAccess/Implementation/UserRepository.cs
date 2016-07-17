namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

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

        public List<UserPublicProfile> LoadAllUsersPublicProfileWithChannelsCount()
        {
            var userPublicProfiles =
               from U in this.database.Users
               join S in this.database.RssChannelsSubscriptions
                   on U.Id equals S.UserId
               select new UserPublicProfile
               {
                   Id = U.Id,
                   DisplayName = U.DisplayName,
                   ChannelsCount = U.RssSubscriptionList.Count
               };

            return userPublicProfiles.Distinct().ToList();
        }

        public User LoadUserPublicProfile(long id)
        {
            var user = this.database.Users
                .Include(x => x.RssSubscriptionList)
                .Include(x => x.RssSubscriptionList.Select(c => c.RssChannel))
                .Include(x => x.EventsRssViewed)
                .Include(x => x.EventsRssViewed.Select(e => e.RssEntry))
                .Single(x => x.Id == id);
            return user;
        }

        public User GetUserPrivateDetails(long currentUserId)
        {
            var user = this.database
                .Users
                .Include(u => u.SocialLogins)
                .Single(u => u.Id == currentUserId);
            return user;
        }
    }
}