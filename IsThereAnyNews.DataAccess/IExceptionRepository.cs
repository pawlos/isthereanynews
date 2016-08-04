namespace IsThereAnyNews.DataAccess
{
    using System.Collections.Generic;

    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IExceptionRepository
    {
        void SaveToDatabase(IEnumerable<ItanException> exceptions);
    }
}