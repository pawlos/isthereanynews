using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;

namespace IsThereAnyNews.Services.Implementation
{
    public class ChannelEventsRepository : IChannelEventsRepository
    {
        private readonly ItanDatabaseContext database;

        public ChannelEventsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<ChannelUpdateEventDto> LoadUpdateEvents(int eventsCount)
        {
            var channelUpdateEventDtos = this.database
                .RssChannelUpdates
                .Include(i => i.RssChannel)
                .OrderByDescending(o => o.Id)
                .Take(eventsCount)
                .Select(c => new ChannelUpdateEventDto
                {
                    Id=c.Id,
                    Updated = c.Created,
                    ChannelTitle = c.RssChannel.Title
                })
                .ToList();

            return channelUpdateEventDtos;
        }
    }
}