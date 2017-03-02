using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IsThereAnyNews.EntityFramework;

namespace IsThereAnyNews.Services.Implementation
{
    public class ExceptionEventsRepository : DataAccess.IExceptionEventsRepository
    {
        private readonly ItanDatabaseContext database;

        public ExceptionEventsRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public List<Dtos.ExceptionEventDto> LoadLatest(int eventCount)
        {
            var exceptionEventDtos = this.database
                .EventException
                .Include(i => i.ItanException)
                .OrderByDescending(o => o.Id)
                .Take(eventCount)
                .Select(s => new Dtos.ExceptionEventDto
                {
                    Id = s.Id,
                    Occured = s.Created,
                    ErrorId = s.ErrorId,
                    Message = s.ItanException.Message,
                    StackTrace = s.ItanException.Stacktrace,
                    Source = s.ItanException.Source,
                    Typeof = s.ItanException.Typeof
                })
                .ToList();
            return exceptionEventDtos;
        }
    }
}