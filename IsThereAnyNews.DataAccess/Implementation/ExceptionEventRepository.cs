namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class ExceptionEventRepository : IExceptionEventRepository
    {
        private readonly ItanDatabaseContext database;

        public ExceptionEventRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(IEnumerable<EventItanException> events)
        {
            this.database.EventException.AddRange(events);
            this.database.SaveChanges();
        }
    }
}