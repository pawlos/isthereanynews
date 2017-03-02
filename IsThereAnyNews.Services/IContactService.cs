namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public interface IContactService
    {
        ContactViewModel GetViewModel();

        void SaveAdministrationContact(ContactAdministrationDto dto);
    }
}

