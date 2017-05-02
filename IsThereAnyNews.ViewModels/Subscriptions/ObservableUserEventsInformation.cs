using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public class ObservableUserEventsInformation: ISubscriptionViewModel
    {
        public string Count { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public StreamType StreamType => StreamType.Person;
        public string IconType => "fa-person";
    }
}