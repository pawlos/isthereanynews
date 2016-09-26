namespace IsThereAnyNews.DataAccess.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class UsersSubscriptionRepository : IUsersSubscriptionRepository
    {
        private readonly ItanDatabaseContext database;

        public UsersSubscriptionRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void CreateNewSubscription(long followerId, long observedId)
        {
            if (this.IsUserSubscribedToUser(followerId, observedId))
            {
                return;
            }

            var userSubscription = new UserSubscription { FollowerId = followerId, ObservedId = observedId };
            this.database.UsersSubscriptions.Add(userSubscription);
            this.database.SaveChanges();
        }

        public List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId)
        {
            var nameAndCountUserSubscriptions = this.database.UsersSubscriptions
                .Include(us => us.Observed)
                .Include(us => us.EntriesToRead)
                .Where(us => us.FollowerId == currentUserId)
                .Select(this.ProjectToNameAndCountUserSubscription)
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
                 .Where(s => s.Id == subscriptionId)
                 .Include(s => s.EntriesToRead)
                 .SelectMany(s => s.EntriesToRead)
                 .Include(s => s.EventRssUserInteraction)
                 .Include(s => s.EventRssUserInteraction.RssEntry)
                 .Where(s => !s.IsRead)
                 .ToList();

            return userSubscriptions.ToList();
        }

        public List<UserSubscriptionEntryToRead> LoadAllEntriesFromSubscription(long subscriptionId)
        {
            var userSubscriptions = this.database.UsersSubscriptions
                           .Where(s => s.Id == subscriptionId)
                           .Include(s => s.EntriesToRead)
                           .SelectMany(s => s.EntriesToRead)
                           .Include(s => s.EventRssUserInteraction)
                           .Include(s => s.EventRssUserInteraction.RssEntry)
                           .ToList();

            return userSubscriptions.ToList();
        }

        public bool IsUserSubscribedToUser(long followerId, long observedId)
        {
            var isSubscribed = this.database
                                   .UsersSubscriptions
                                   .Any(s => s.FollowerId == followerId && s.ObservedId == observedId);
            return isSubscribed;
        }

        public void DeleteUserSubscription(long followerId, long observedId)
        {
            if (this.IsUserSubscribedToUser(followerId, observedId) == false)
            {
                return;
            }

            var userSubscription = this.database
                                       .UsersSubscriptions
                                       .Single(x => x.FollowerId == followerId && x.ObservedId == observedId);
            this.database.UsersSubscriptions.Remove(userSubscription);
            this.database.SaveChanges();
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
                Count = arg.EntriesToRead.Count(x => !x.IsRead)
            };
        }
    }
}