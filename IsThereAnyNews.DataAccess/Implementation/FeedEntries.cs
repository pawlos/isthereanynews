namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;
    using IsThereAnyNews.ProjectionModels;

    public class FeedEntries
    {
        public List<RssEntryDTO> RssEntryDtos { get; }

        public FeedEntries(List<RssEntryDTO> rssEntryDtos)
        {
            this.RssEntryDtos = rssEntryDtos;
        }
    }
}