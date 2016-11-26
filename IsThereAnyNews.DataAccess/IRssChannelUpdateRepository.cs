namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelUpdateRepository
    {
        void SaveEvent(long eventRssChannelUpdated);
    }
}