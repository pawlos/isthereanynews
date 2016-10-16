namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.EntityFramework.Models.Entities;
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

        [HttpGet]
        public ViewResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        public JsonResult ConfigurationStatus()
        {
            var x =
                new
                {
                    UserRegistration = RegistrationSupported.Closed,
                    UserLimit = 198312,
                    CurrentUsers = 33,
                    Subscriptions = 3232,
                    RssNews = 9393939393,
                };

            return this.Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}