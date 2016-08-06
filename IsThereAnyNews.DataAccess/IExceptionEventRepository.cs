namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Events;

    public interface IExceptionEventRepository
    {
        void SaveToDatabase(IEnumerable<EventItanException> events);
    }
}