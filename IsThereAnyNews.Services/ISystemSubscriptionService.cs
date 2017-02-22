using System.Collections.Generic;
using IsThereAnyNews.ViewModels;

namespace IsThereAnyNews.Services
{
    public interface ISystemSubscriptionService
    {
        List<ChannelEventViewModel> LoadEvents();
    }
}