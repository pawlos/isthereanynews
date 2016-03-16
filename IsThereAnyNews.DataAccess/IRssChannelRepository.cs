using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelRepository
    {
        RssChannel LoadRssChannel(long id);
        void UpdateRssLastUpdateTimeToDatabase(List<RssChannel> rssChannels);
    }
}