using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public class ObservableUserEventsInformation: ISubscriptionViewModel
    {
        public int Count { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public StreamType StreamType => StreamType.Person;
        public string IconType => "fa-user-circle";
    }
}