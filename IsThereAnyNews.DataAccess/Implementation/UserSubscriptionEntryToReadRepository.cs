namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Linq;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class UserSubscriptionEntryToReadRepository : IUserSubscriptionEntryToReadRepository
    {
        private readonly ItanDatabaseContext database;

        public UserSubscriptionEntryToReadRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void CopyAllUnreadElementsToUser(long currentUserId)
        {
            var observedUsersId = this.database
                .UsersSubscriptions
                .Where(x => x.FollowerId == currentUserId)
                .ToList();

            var currentUserObservedUsersIds = observedUsersId.Select(x => x.ObservedId).ToList();

            var lastReadTime = this.database.Users.Single(x => x.Id == currentUserId).LastReadTime;

            var eventRssVieweds = this.database
                .EventsRssUserInteraction
                .Where(x => currentUserObservedUsersIds.Contains(x.UserId))
                .Where(x => x.Created >= lastReadTime)
                .ToList();

            foreach (var rssViewed in eventRssVieweds)
            {
                var userSubscriptionEntryToRead = new UserSubscriptionEntryToRead
                {
                    EventRssViewedId = rssViewed.Id,
                    UserSubscriptionId = observedUsersId.Single(o => o.ObservedId == rssViewed.UserId).Id
                };
                this.database.UsersSubscriptionsToRead.Add(userSubscriptionEntryToRead);
            }

            this.database.SaveChanges();
        }
    }
}