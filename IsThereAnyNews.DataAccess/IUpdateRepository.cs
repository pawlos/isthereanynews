using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IUpdateRepository
    {
        List<RssChannel> LoadAllGlobalRssChannels();
    }
}