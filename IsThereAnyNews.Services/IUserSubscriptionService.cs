using IsThereAnyNews.ViewModels;
using System.Collections.Generic;

namespace IsThereAnyNews.Services
{
    public interface IUserSubscriptionService
    {
        List<ObservableUserEventsInformation> LoadAllObservableSubscription();
    }
}