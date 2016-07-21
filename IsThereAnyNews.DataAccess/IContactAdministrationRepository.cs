namespace IsThereAnyNews.DataAccess
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public interface IContactAdministrationRepository
    {
        void SaveToDatabase(ContactAdministration entity);
    }
}