using System.Collections.Generic;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.DataAccess.Implementation
{
    class RssEntryRepository : IRssEntryRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEntryRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(List<RssEntry> rssEntriesList)
        {
            this.database.RssEntries.AddRange(rssEntriesList);
            this.database.SaveChanges();
        }
    }
}