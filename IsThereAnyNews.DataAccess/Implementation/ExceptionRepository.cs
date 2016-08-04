namespace IsThereAnyNews.DataAccess.Implementation
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class ExceptionRepository : IExceptionRepository
    {
        private readonly ItanDatabaseContext database;

        public ExceptionRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(IEnumerable<ItanException> exceptions)
        {
            this.database.ItanExceptions.AddRange(exceptions);
            this.database.SaveChanges();
        }
    }
}