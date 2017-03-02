namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;
    using Dtos;

    public interface IChannelEventsRepository
    {
        List<ChannelUpdateEventDto> LoadUpdateEvents(int eventsCount);
    }
}