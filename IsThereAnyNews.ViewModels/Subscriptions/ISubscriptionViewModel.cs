using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public interface ISubscriptionViewModel
    {
        int Count { get; }

        long Id { get; }

        string Name { get; }

        StreamType StreamType { get; }

        string IconType { get; }
    }
}