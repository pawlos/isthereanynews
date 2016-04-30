using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
{
    public class UserDetailedPublicProfileViewModel
    {
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
        public List<PublicProfileChannelInformation> Channels { get; set; }
        public List<EventRssViewedViewModel> Events { get; set; }
        public int EventsCount { get; set; }
        public long ViewingUserId { get; set; }
    }
}