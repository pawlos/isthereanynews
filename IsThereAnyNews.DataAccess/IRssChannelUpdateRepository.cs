using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelUpdateRepository
    {
        void SaveEvent(EventRssChannelUpdated eventRssChannelUpdated);
    }
}