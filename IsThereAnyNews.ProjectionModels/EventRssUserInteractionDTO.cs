namespace IsThereAnyNews.ProjectionModels
{
    using System;

    public class EventRssUserInteractionDTO
    {
        public string Title { get; set; }
        public DateTime Viewed { get; set; }
        public long RssId { get; set; }
    }
}