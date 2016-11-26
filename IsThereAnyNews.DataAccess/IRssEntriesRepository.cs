namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.Dtos;

    public interface IRssEntriesRepository
    {
        void SaveToDatabase(List<NewRssEntryDTO> rssEntriesList);
    }
}