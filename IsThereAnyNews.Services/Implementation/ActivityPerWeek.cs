using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.Services.Implementation
{
    public class ActivityPerWeek
    {
        public int WeekNumber { get; set; }
        public int RssCount { get; set; }
        public List<EventRssUserInteraction> RssVieweds { get; set; }

        public ActivityPerWeek(int weekNumber, int rssCount, List<EventRssUserInteraction> rssVieweds)
        {
            WeekNumber = weekNumber;
            RssCount = rssCount;
            RssVieweds = rssVieweds;
        }
    }
}