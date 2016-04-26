namespace IsThereAnyNews.DataAccess
{
    public interface IUserSubscriptionEntryToReadRepository
    {
        void CopyAllUnreadElementsToUser(long currentUserId);
    }
}