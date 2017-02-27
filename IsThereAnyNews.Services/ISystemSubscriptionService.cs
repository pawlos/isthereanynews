using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface ISystemSubscriptionService
    {
        AdminEventsViewModel LoadEvents();
    }
}