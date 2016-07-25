namespace IsThereAnyNews.Services.Implementation
{
    using AutoMapper;

    using IsThereAnyNews.DataAccess;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;
    using IsThereAnyNews.EntityFramework.Models.Events;
    using IsThereAnyNews.ViewModels;

    public class ContactService : IContactService
    {
        private readonly IMapper mapper;
        private readonly IContactAdministrationRepository repositoryContactAdministration;
        private readonly IContactAdministrationEventRepository eventsContactAdministration;

        public ContactService(
            IMapper mapper, 
            IContactAdministrationRepository repositoryContactAdministration, 
            IContactAdministrationEventRepository eventsContactAdministration)
        {
            this.mapper = mapper;
            this.repositoryContactAdministration = repositoryContactAdministration;
            this.eventsContactAdministration = eventsContactAdministration;
        }

        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }

        public void SaveAdministrationContact(ContactAdministrationModel model)
        {
            var entity = this.mapper.Map<ContactAdministration>(model);
            this.repositoryContactAdministration.SaveToDatabase(entity);
            var contactEvent = this.mapper.Map<ContactAdministrationEvent>(entity);
            this.eventsContactAdministration.SaveToDatabase(contactEvent);
        }
    }
}