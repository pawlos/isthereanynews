using System.Collections.Generic;

namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public interface ISubscriptionHandler
    {
        RssSubscriptionIndexViewModel GetSubscriptionViewModel(long subscriptionId, ShowReadEntries showReadEntries);
        void MarkRead(System.Collections.Generic.List<long> displayedItems);
        void AddEventViewed(long dtoId);
        void MarkSkipped(long modelSubscriptionId, List<long> ids);
        void MarkRead(long userId, long rssId, long dtoSubscriptionId);
    }
}