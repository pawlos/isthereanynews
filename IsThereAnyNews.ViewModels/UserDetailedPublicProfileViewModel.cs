namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class UserDetailedPublicProfileViewModel
    {
        public string DisplayName { get; set; }
        public int ChannelsCount { get; set; }
        public List<string> Channels { get; set; }
        public List<PublicProfileChannelInformation> Users { get; set; }
        public List<EventRssViewedViewModel> Events { get; set; }
        public int EventsCount { get; set; }
        public long ViewingUserId { get; set; }

        public bool IsUserAlreadySubscribed { get; set; }
    }
}