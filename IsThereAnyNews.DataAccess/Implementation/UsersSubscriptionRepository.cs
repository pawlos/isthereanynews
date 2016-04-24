using IsThereAnyNews.DataAccess;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.Services.Implementation
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
    }
}