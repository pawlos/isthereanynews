namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class UserDetailedPublicProfileViewModel
    {
        public List<string> Channels { get; set; }

        public int ChannelsCount { get; set; }

        public string DisplayName { get; set; }

        public List<EventRssViewedViewModel> Events { get; set; }

        public int EventsCount { get; set; }

        public bool IsUserAlreadySubscribed { get; set; }

        public List<PublicProfileChannelInformation> Users { get; set; }

        public long ViewingUserId { get; set; }
    }
}