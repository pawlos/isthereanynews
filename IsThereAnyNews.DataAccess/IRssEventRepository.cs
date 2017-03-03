namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    public interface IRssEventRepository
    {
        void AddEventRssViewed(long currentUserId, long rssToReadId);
        void MarkRead(List<long> ids);
        void MarkClicked(long id, long currentUserId);
        void AddEventRssSkipped(long cui, List<long> id);
    }
}