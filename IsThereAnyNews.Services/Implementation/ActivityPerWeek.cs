namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Events;

    public class ActivityPerWeek
    {
        public ActivityPerWeek(int weekNumber, int rssCount, List<EventRssUserInteraction> rssVieweds)
        {
            this.WeekNumber = weekNumber;
            this.RssCount = rssCount;
            this.RssVieweds = rssVieweds;
        }

        public int RssCount { get; set; }
        public List<EventRssUserInteraction> RssVieweds { get; set; }
        public int WeekNumber { get; set; }
    }
}