namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using IsThereAnyNews.EntityFramework;

    public class ChannelEventsRepository : DataAccess.IChannelEventsRepository
    {
        private readonly ItanDatabaseContext database;

        public ChannelEventsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<Dtos.ChannelUpdateEventDto> LoadUpdateEvents(int eventsCount)
        {
            var channelUpdateEventDtos = this.database
                .RssChannelUpdates
                .Include(i => i.RssChannel)
                .OrderByDescending(o => o.Id)
                .Take(eventsCount)
                .Select(c => new Dtos.ChannelUpdateEventDto
                {
                    Id = c.Id,
                    Updated = c.Created,
                    ChannelTitle = c.RssChannel.Title
                })
                .ToList();

            return channelUpdateEventDtos;
        }
    }
}