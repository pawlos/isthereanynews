using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess.Implementation
{
    class UsersSubscriptionRepository : IUsersSubscriptionRepository
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
                .Include(us=>us.Observed)
                .Include(us=>us.EntriesToRead)
                .Where(us => us.FollowerId == currentUserId)
                .Select(ProjectToNameAndCountUserSubscription)
                .ToList();
            return nameAndCountUserSubscriptions;
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