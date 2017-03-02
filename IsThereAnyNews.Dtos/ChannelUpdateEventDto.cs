namespace IsThereAnyNews.Dtos
{
    using System;

    public class ChannelUpdateEventDto
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public string ChannelTitle { get; set; }
    }
}