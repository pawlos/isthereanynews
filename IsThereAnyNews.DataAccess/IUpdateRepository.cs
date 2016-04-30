using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess
{
    public interface IUpdateRepository
    {
        List<RssChannel> LoadAllGlobalRssChannels();
    }
}