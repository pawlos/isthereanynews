namespace IsThereAnyNews.ViewModels
{
    public class AdminEventsViewModel
    {
        public AdminEventsViewModel()
        {
            this.Creations= new ChannelEventCreationViewModel();
            this.Exceptions= new ChannelEventCreationViewModel();
            this.Updates= new ChannelEventCreationViewModel();
        }
        public ChannelEventViewModel Creations { get; set; }

        public ChannelEventViewModel Exceptions { get; set; }

        public ChannelEventViewModel Updates { get; set; }
    }
}