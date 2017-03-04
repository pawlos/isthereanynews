namespace IsThereAnyNews.ViewModels
{
    public class AdminEventsViewModel
    {
        public ChannelEventViewModel Creations { get; set; }

        public ChannelEventViewModel Exceptions { get; set; }

        public ChannelEventViewModel Updates { get; set; }
    }
}