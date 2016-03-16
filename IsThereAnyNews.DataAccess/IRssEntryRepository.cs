using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess
{
    public interface IRssEntryRepository
    {
        void SaveToDatabase(List<RssEntry> rssEntriesList);
    }
}