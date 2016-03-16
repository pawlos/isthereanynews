using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssEntriesRepository
    {
        void SaveToDatabase(List<RssEntry> rssEntriesList);
    }
}