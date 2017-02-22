namespace IsThereAnyNews.ViewModels
{
    using IsThereAnyNews.SharedData;

    public class ObservableUserEventsInformation
    {
        public string Count { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public StreamType StreamType => StreamType.Person;
    }
}