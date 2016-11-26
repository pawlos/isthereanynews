namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class ContactService : IContactService
    {
        private readonly IContactAdministrationRepository repositoryContactAdministration;
        private readonly IContactAdministrationEventRepository eventsContactAdministration;

        public ContactService(IContactAdministrationRepository repositoryContactAdministration,
            IContactAdministrationEventRepository eventsContactAdministration)
        {
            this.repositoryContactAdministration = repositoryContactAdministration;
            this.eventsContactAdministration = eventsContactAdministration;
        }

        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }

        public void SaveAdministrationContact(ContactAdministrationDto dto)
        {
            var contactId = this.repositoryContactAdministration.SaveToDatabase(dto);
            this.eventsContactAdministration.SaveToDatabase(contactId);
        }
    }
}