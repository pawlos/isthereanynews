namespace IsThereAnyNews.ProjectionModels
{
    using System.Collections.Generic;

    public class UserPublicProfileDto
    {
        public UserPublicProfileDto()
        {
            this.Events = new List<EventRssUserInteractionDTO>();
        }

        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
        public List<string> Channels { get; set; }
        public List<EventRssUserInteractionDTO> Events { get; set; }

        public long ViewingUserId { get; set; }
        public int EventsCount => this.Events.Count;
        public long Id { get; set; }
    }
}