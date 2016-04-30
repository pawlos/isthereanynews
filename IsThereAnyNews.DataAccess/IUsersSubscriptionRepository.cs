using System.Collections.Generic;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.DataAccess
{
    public interface IUsersSubscriptionRepository
    {
        void CreateNewSubscription(long followerId, long observedId);
        List<NameAndCountUserSubscription> LoadNameAndCountForUser(long currentUserId);
    }

}