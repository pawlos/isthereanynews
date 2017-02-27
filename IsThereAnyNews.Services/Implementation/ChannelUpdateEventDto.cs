using System;

namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelUpdateEventDto
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public string ChannelTitle { get; set; }
    }
}