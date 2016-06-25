using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models.Events;

namespace IsThereAnyNews.Services.Implementation
{
    public class ActivityPerWeek
    {
        public int WeekNumber { get; set; }
        public int RssCount { get; set; }
        public List<EventRssViewed> RssVieweds { get; set; }

        public ActivityPerWeek(int weekNumber, int rssCount, List<EventRssViewed> rssVieweds)
        {
            WeekNumber = weekNumber;
            RssCount = rssCount;
            RssVieweds = rssVieweds;
        }
    }
}