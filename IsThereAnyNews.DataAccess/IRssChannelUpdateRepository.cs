namespace IsThereAnyNews.DataAccess
{
    using System;

    public interface IRssChannelUpdateRepository
    {
        void SaveEvent(long eventRssChannelUpdated);

        DateTime GetLatestUpdateDate(long rssChannelId);
    }
}