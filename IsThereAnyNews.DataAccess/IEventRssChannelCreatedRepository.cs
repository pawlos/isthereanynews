using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess
{
    public interface IEventRssChannelCreatedRepository
    {
        void SaveToDatabase(EventRssChannelCreated eventRssChannelCreated);
    }
}