namespace IsThereAnyNews.Services.Handlers
{
    using System.Collections.Generic;

    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;

    public interface ISubscriptionHandler
    {
        ISubscriptionContentIndexViewModel GetSubscriptionViewModel(long userId, long subscriptionId, ShowReadEntries showReadEntries);
        void MarkNavigated(long userId, long rssId, long dtoSubscriptionId);
        void MarkClicked(long cui, long id, long subscriptionId);
        void MarkSkipped(long cui, long subscriptionId, List<long> entries);
    }
}