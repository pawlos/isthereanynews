using System;

namespace IsThereAnyNews.ViewModels
{
    public class EventRssViewedViewModel
    {
        public string Title { get; set; }
        public DateTime Viewed { get; set; }
        public long RssId { get; set; }
    }
}