using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class UsersSubscriptionRepository : IUsersSubscriptionRepository
    {
        private readonly ItanDatabaseContext database;

        public UsersSubscriptionRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void CreateNewSubscription(long followerId, long observedId)
        {
            var userSubscription = new UserSubscription
            {
                FollowerId = followerId,
                ObservedId = observedId
            };

            this.database.UsersSubscriptions.Add(userSubscription);
            this.database.SaveChanges();
        }

        public List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId)
        {
            var nameAndCountUserSubscriptions = this.database.UsersSubscriptions
                .Include(us => us.Observed)
                .Include(us => us.EntriesToRead)
                .Where(us => us.FollowerId == currentUserId)
                .Select(ProjectToNameAndCountUserSubscription)
                .ToList();
            return nameAndCountUserSubscriptions;
        }

        public void UpdateUserLastReadTime(long currentUserId, DateTime now)
        {
            this.database.Users.Single(u => u.Id == currentUserId).LastReadTime = now;
            this.database.SaveChanges();
        }

        public bool DoesUserOwnsSubscription(long subscriptionId, long currentUserId)
        {
            var subscription = this.database.UsersSubscriptions
                .Where(x => x.Id == subscriptionId)
                .SingleOrDefault(x => x.FollowerId == currentUserId);
            return subscription != null;
        }

        public List<UserSubscriptionEntryToRead> LoadAllUnreadEntriesFromSubscription(long subscriptionId)
        {
            var userSubscriptions = this.database.UsersSubscriptions
                .Include(s => s.EntriesToRead)
                .Include(s => s.EntriesToRead.Select(e => e.EventRssViewed))
                .Include(s => s.EntriesToRead.Select(e => e.EventRssViewed).Select(r => r.RssEntry))
                .Where(s => s.Id == subscriptionId)
                .ToList();
            return userSubscriptions.SelectMany(x => x.EntriesToRead).Distinct().ToList();
        }

        public RssChannelSubscription LoadChannelInformation(long subscriptionId)
        {
            var userSubscription =
                this.database
                    .UsersSubscriptions
                    .Include(x => x.Observed)
                    .Single(x => x.Id == subscriptionId);

            var rssChannelSubscription = new RssChannelSubscription
            {
                Title = userSubscription.Observed.DisplayName,
                Created = userSubscription.Created
            };

            return rssChannelSubscription;
        }

        private NameAndCountUserSubscription ProjectToNameAndCountUserSubscription(UserSubscription arg)
        {
            return new NameAndCountUserSubscription
            {
                Id = arg.Id,
                Name = arg.Observed.DisplayName,
                Count = arg.EntriesToRead.Count
            };
        }
    }
}