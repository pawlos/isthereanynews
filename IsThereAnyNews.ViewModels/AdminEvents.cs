namespace IsThereAnyNews.ViewModels
{
    public class AdminEventsViewModel
    {
        public ChannelEventViewModel Updates { get; set; }
        public ChannelEventViewModel Creations { get; set; }
        public ChannelEventViewModel Exceptions { get; set; }
    }
}