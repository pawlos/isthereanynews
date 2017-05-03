﻿using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.ViewModels.Subscriptions
{
    public interface ISubscriptionViewModel
    {
        string Count { get; }

        long Id { get; }

        string Name { get; }

        StreamType StreamType { get; }

        string IconType { get; }
    }
}