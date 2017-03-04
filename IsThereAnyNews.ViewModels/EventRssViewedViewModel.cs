namespace IsThereAnyNews.ViewModels
{
    using System;

    public class EventRssViewedViewModel
    {
        public long RssId { get; set; }

        public string Title { get; set; }

        public DateTime Viewed { get; set; }
    }
}