namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;

    using AutoMapper.QueryableExtensions;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.ProjectionModels;

    public class UserRepository : IUserRepository
    {
        private readonly ItanDatabaseContext database;

        public UserRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public long CreateNewUser(string name, string email)
        {
            var user = new User
            {
                DisplayName = name,
                Email = email
            };

            this.database.Users.Add(user);
            this.database.SaveChanges();
            return user.Id;
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

        public UserPublicProfileDto LoadUserPublicProfile(long id)
        {
            var users =
                from u in
                this.database.Users
                    .Include(x => x.RssSubscriptionList)
                    .Include(x => x.RssSubscriptionList.Select(xx => xx.RssChannel))
                    .Include(x => x.EventsRssViewed)
                    .Include(x => x.EventsRssViewed.Select(xx => xx.RssEntry))
                where u.Id == id
                select
                new UserPublicProfileDto
                {
                    Id = u.Id,
                    Channels = u.RssSubscriptionList.Select(x => x.Title).ToList(),
                    DisplayName = u.DisplayName,
                    ChannelsCount = u.RssSubscriptionList.Count,
                    Events = u.EventsRssViewed.Select(e => new EventRssUserInteractionDTO
                    {
                        Title = e.RssEntry.Title,
                        RssId = e.RssEntryId,
                        Viewed = e.Created
                    }).ToList(),
                };


            var d = users.Single();
            return d;
        }

        public UserPrivateProfileDto GetUserPrivateDetails(long currentUserId)
        {
            var user = this.database
                .Users
                .Include(u => u.SocialLogins)
                .Where(u => u.Id == currentUserId)
                .ProjectTo<UserPrivateProfileDto>()
                .Single();
            return user;
        }

        public void ChangeEmail(long currentUserId, string email)
        {
            var single = this.database.Users.Single(u => u.Id == currentUserId);
            single.Email = email;
            this.database.SaveChanges();
        }

        public void ChangeDisplayName(long currentUserId, string displayname)
        {
            var single = this.database.Users.Single(u => u.Id == currentUserId);
            single.DisplayName = displayname;
            this.database.SaveChanges();
        }
    }

    public class X
    {
        public User User { get; set; }

        public IEnumerable<RssChannelSubscription> Subs { get; set; }
    }
}