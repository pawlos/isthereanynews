namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IUpdateRepository
    {
        List<RssChannel> LoadAllGlobalRssChannelsSortedByUpdate();
    }
}