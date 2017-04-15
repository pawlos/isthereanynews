namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Web.Interfaces.Services;

    [Authorize]
    public partial class AccountController: Controller
    {
        private readonly IService accountService;

        public AccountController(IService accountService)
        {
            this.accountService = accountService;
        }

    }
}