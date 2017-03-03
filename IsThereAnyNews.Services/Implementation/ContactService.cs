namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public class ContactService : IContactService
    {
        private readonly IContactAdministrationEventRepository eventsContactAdministration;
        private readonly IContactAdministrationRepository repositoryContactAdministration;
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
            this.eventsContactAdministration.SaveContactAdministrationEventEventToDatabase(contactId);
        }
    }
}