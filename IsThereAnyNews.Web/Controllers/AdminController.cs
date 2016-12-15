namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.RssChannelUpdater;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;

    [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
    public class AdminController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IUpdateService updateService;

        public AdminController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IAdminService adminService,
            IUpdateService updateService)
            : base(authentication, loginService)
        {
            this.adminService = adminService;
            this.updateService = updateService;
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

        [HttpPost]
        public HttpStatusCodeResult SpinUpdateJob()
        {
            Task.Run(() => this.updateService.UpdateGlobalRss());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}