namespace IsThereAnyNews.Services.Handlers
{
    using System.Collections.Generic;

    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public interface ISubscriptionHandler
    {
        RssSubscriptionIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries);
        void MarkRead(List<long> displayedItems);
        void MarkSkipped(long modelSubscriptionId, List<long> ids);
        void MarkRead(long userId, long rssId, long dtoSubscriptionId);
        void AddEventViewed(long cui, long id);
        void AddEventSkipped(long cui, string entries);
    }
}