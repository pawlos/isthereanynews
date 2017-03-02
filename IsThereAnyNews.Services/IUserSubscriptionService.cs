namespace IsThereAnyNews.Services
{
    using System.Collections.Generic;

    using IsThereAnyNews.ViewModels;

    public interface IUserSubscriptionService
    {
        List<ObservableUserEventsInformation> LoadAllObservableSubscription();
    }
}