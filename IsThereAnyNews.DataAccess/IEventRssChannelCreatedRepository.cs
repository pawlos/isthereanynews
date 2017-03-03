namespace IsThereAnyNews.DataAccess
{
    public interface IEventRssChannelCreatedRepository
    {
        void SaveChannelCreatedEventToDatabase(long eventRssChannelCreated);
    }
}