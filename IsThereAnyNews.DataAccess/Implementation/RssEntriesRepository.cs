namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;

    using AutoMapper;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class RssEntriesRepository : IRssEntriesRepository
    {
        private readonly ItanDatabaseContext database;
        private readonly IMapper mapper;

        public RssEntriesRepository(ItanDatabaseContext database, IMapper mapper)
        {
            this.database = database;
            this.mapper = mapper;
        }

        public void SaveToDatabase(List<NewRssEntryDTO> rssEntriesList)
        {
            var rssEntries = this.mapper.Map<List<NewRssEntryDTO>, List<RssEntry>>(rssEntriesList);
            this.database.RssEntries.AddRange(rssEntries);
            this.database.SaveChanges();
        }
    }
}