namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class ContactService : IContactService
    {
        private readonly IEntityRepository entityRepository;

        public ContactService(IEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }

        public void SaveAdministrationContact(ContactAdministrationDto dto)
        {
            var contactId = this.entityRepository.SaveToDatabase(dto);
            this.entityRepository.SaveContactAdministrationEventEventToDatabase(contactId);
        }
    }
}