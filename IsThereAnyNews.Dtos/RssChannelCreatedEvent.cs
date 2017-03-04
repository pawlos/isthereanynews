namespace IsThereAnyNews.Dtos
{
    using System;

    public class RssChannelCreatedEvent
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public long ChannelId { get; set; }
        public string ChannelTitle { get; set; }
    }
}