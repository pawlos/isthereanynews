using System.Collections.Generic;
using IsThereAnyNews.Mvc.Models;

namespace IsThereAnyNews.Mvc.Repositories
{
    public interface IRssChannelRepository
    {
        List<RssChannel> LoadAllChannels();
        RssChannel Load(long id);
        List<RssChannel> LoadAllChannelsForUser(string currentUserId);
    }
}