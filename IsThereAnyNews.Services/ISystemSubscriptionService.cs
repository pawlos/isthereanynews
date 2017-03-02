namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.ViewModels;

    public interface ISystemSubscriptionService
    {
        AdminEventsViewModel LoadEvents();
    }
}