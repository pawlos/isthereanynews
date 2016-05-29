namespace IsThereAnyNews.ViewModels
{
    public class RssEntryToReadViewModel
    {
        public RssEntryViewModel RssEntryViewModel { get; set; }
        public bool IsRead { get; set; }
        public long Id { get; set; }
        public string Url { get; set; }
    }
}