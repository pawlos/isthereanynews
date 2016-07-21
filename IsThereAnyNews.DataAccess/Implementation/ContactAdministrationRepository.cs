namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class ContactAdministrationRepository : IContactAdministrationRepository
    {
        private readonly ItanDatabaseContext database;

        public ContactAdministrationRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(ContactAdministration entity)
        {
            this.database.ContactsAdministration.Add(entity);
            this.database.SaveChanges();
        }
    }
}