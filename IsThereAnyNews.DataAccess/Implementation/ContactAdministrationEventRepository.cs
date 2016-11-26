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

        public void SaveToDatabase(long contactId)
        {
            var contactAdministrationEvent = new ContactAdministrationEvent { ContactAdministrationId = contactId };
            this.database.ContactsAdministrationEvents.Add(contactAdministrationEvent);
            this.database.SaveChanges();
        }
    }
}