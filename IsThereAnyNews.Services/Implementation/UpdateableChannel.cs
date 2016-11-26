namespace IsThereAnyNews.Services.Implementation
{
    using System;

    public class UpdateableChannel
    {
        public string Url { get; set; }

        public DateTimeOffset RssLastUpdatedTime { get; set; }

        public long Id { get; set; }

        public DateTime Updated { get; set; }
    }
}