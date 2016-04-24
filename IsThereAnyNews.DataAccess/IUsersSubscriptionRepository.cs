namespace IsThereAnyNews.DataAccess
{
    public interface IUsersSubscriptionRepository
    {
        void CreateNewSubscription(long followerId, long observedId);
    }

}