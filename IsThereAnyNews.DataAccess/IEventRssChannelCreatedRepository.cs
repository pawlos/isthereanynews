namespace IsThereAnyNews.DataAccess
{
    public interface IEventRssChannelCreatedRepository
    {
        void SaveToDatabase(long eventRssChannelCreated);
    }
}