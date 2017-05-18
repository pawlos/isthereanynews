namespace IsThereAnyNews.DataAccess
{
    using System;

    public class ChannelCreateEventDto
    {
        public long Id { get; set; }
        public DateTime Updated { get; set; }
        public string ChannelTitle { get; set; }
    }
}