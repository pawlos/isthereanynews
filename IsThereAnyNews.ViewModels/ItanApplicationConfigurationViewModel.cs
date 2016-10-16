namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ItanApplicationConfigurationViewModel
    {
        public RegistrationSupported UserRegistration { get; set; }

        public long UserLimit { get; set; }

        public long CurrentUsers { get; set; }

        public long Subscriptions { get; set; }

        public long RssNews { get; set; }
    }
}