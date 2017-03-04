namespace IsThereAnyNews.ViewModels
{
    public class RssEntryToReadViewModel
    {
        public long Id { get; set; }

        public bool IsRead { get; set; }

        public RssEntryViewModel RssEntryViewModel { get; set; }
    }
}