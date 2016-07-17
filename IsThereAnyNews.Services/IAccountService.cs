namespace IsThereAnyNews.Mvc.Controllers
{
    using IsThereAnyNews.ViewModels;

    public interface IAccountService
    {
        AccountDetailsViewModel GetAccountDetailsViewModel();
    }
}