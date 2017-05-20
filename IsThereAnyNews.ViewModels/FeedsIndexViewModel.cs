namespace IsThereAnyNews.ViewModels
{
    public class FeedsIndexViewModel
    {
        public int Count { get; }

        public FeedsIndexViewModel(int count)
        {
            this.Count = count;
        }
    }
}