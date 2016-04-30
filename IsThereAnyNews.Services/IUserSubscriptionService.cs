using System.Collections.Generic;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Mvc.Controllers
{
    public interface IUserSubscriptionService
    {
        List<ObservableUserEventsInformation> LoadAllObservableSubscription();
    }
}