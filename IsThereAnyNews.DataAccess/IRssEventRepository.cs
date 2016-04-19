namespace IsThereAnyNews.DataAccess
{
    public interface IRssEventRepository
    {
        void AddEventRssViewed(long currentUserId, long rssToReadId);
    }
}