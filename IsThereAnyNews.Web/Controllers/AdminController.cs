namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Mvc.Infrastructure;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;

    [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;

        public AdminController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IAdminService adminService)
            : base(authentication, loginService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        public JsonResult ConfigurationStatus()
        {
            var configurationStatus = this.adminService.ReadConfiguration();
            return this.Json(configurationStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCodeResult ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.adminService.ChangeRegistration(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.adminService.ChangeUsersLimit(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}