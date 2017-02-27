using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
{
    public interface IChannelEventsRepository
    {
        List<ChannelUpdateEventDto> LoadUpdateEvents(int eventsCount);
    }
}