namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public interface ISubscriptionHandler
    {
        RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries);
        void MarkRead(string displayedItems);

        void AddEventViewed(long dtoId);
    }
}