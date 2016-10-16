namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Mvc.Infrastructure;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;

    [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
    public class AdminController : BaseController
    {
        public AdminController(
            IUserAuthentication authentication,
            ILoginService loginService,
            ISessionProvider sessionProvider)
            : base(authentication, loginService, sessionProvider)
        {
        }

        public ViewResult Index()
        {
            return this.View("Index");
        }
    }
}