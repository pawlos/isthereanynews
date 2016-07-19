namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.ViewModels;

    public interface IContactService
    {
        ContactViewModel GetViewModel();
    }
}