using System.Collections.Generic;
using IsThereAnyNews.EntityFramework;
using IsThereAnyNews.EntityFramework.Models;
using IsThereAnyNews.EntityFramework.Models.Entities;

namespace IsThereAnyNews.DataAccess.Implementation
{
    public class RssEntriesRepository : IRssEntriesRepository
    {
        private readonly ItanDatabaseContext database;

        public RssEntriesRepository(ItanDatabaseContext database)
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