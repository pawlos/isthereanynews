namespace IsThereAnyNews.Services
{
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.ViewModels;

    public interface IAccountService
    {
        AccountDetailsViewModel GetAccountDetailsViewModel();

        void ChangeEmail(ChangeEmailModelDto model);

        void ChangeDisplayName(ChangeDisplayNameModelDto model);
    }
}