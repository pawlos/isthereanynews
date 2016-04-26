using System.Collections.Generic;
using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.DataAccess
{
    public interface IUsersSubscriptionRepository
    {
        void CreateNewSubscription(long followerId, long observedId);
        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);
    }

}