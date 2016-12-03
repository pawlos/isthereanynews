namespace IsThereAnyNews.ProjectionModels
{
    using System;
    public class RssChannelForUpdateDTO
    {
        public string Url { get; set; }
        public DateTime RssLastUpdatedTime { get; set; }
        public long Id { get; set; }
    }
}