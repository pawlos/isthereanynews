namespace IsThereAnyNews.DataAccess.Implementation
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class ContactAdministrationRepository : IContactAdministrationRepository
    {
        private readonly ItanDatabaseContext database;

        public ContactAdministrationRepository(ItanDatabaseContext database)
        {
            this.database = database;
        }

        public long SaveToDatabase(ContactAdministrationDto entity)
        {
            var contactAdministration = new ContactAdministration
                                            {
                                                Email = entity.Email,
                                                Message = entity.Message,
                                                Name = entity.Name,
                                                Topic = entity.Topic
                                            };
            this.database.ContactsAdministration.Add(contactAdministration);
            this.database.SaveChanges();
            return contactAdministration.Id;
        }
    }
}