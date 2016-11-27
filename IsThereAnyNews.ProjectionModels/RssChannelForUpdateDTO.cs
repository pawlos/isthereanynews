namespace IsThereAnyNews.ProjectionModels
{
    using System;
    using System.Collections.Generic;

    public class RssChannelForUpdateDTO
    {
        public string Url { get; set; }

        public DateTimeOffset RssLastUpdatedTime { get; set; }

        public long Id { get; set; }

        public List<EventRssChannelUpdatedDTO> Updates { get; set; }
    }
}