using System.Collections.Generic;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface IUserSubscriptionService
    {
        List<ObservableUserEventsInformation> LoadAllObservableSubscription();
    }
}