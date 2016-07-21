namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Events;

    public class ContactAdministrationEventRepository : IContactAdministrationEventRepository
    {
        private readonly ItanDatabaseContext database;

        public ContactAdministrationEventRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public void SaveToDatabase(ContactAdministrationEvent contactEvent)
        {
            this.database.ContactsAdministrationEvents.Add(contactEvent);
            this.database.SaveChanges();
        }
    }
}