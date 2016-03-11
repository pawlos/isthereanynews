using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssChannelRepository
    {
        List<RssChannel> LoadAllChannels();
        RssChannel Load(long id);
        List<RssChannel> LoadAllChannelsForUser(string currentUserId);
    }
}