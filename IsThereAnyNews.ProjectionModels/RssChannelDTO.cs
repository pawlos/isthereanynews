namespace IsThereAnyNews.ProjectionModels
{
    using System;
    using System.Collections.Generic;

    public class RssChannelDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Added { get; set; }
        public long ChannelId { get; set; }
        public List<RssEntryDTO> Entries { get; set; }
        public DateTime Updated { get; set; }
    }
}