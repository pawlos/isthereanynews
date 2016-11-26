namespace IsThereAnyNews.ProjectionModels
{
    using System.Collections.Generic;

    public class UserPublicProfileDto
    {
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
        public List<RssChannelSubscriptionDTO> Channels { get; set; }
        public List<EventRssUserInteractionDTO> Events { get; set; }
        public int EventsCount { get; set; }
        public long Id { get; set; }
    }
}