namespace IsThereAnyNews.Services.Implementation
{
    using IsThereAnyNews.ViewModels;

    public class ContactService : IContactService
    {
        public ContactViewModel GetViewModel()
        {
            return new ContactViewModel();
        }
    }
}