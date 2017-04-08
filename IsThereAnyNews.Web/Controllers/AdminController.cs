namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Infrastructure;
    using IsThereAnyNews.Web.Interfaces.Services;

    [RoleAuthorize(Roles = new[] {ItanRole.SuperAdmin})]
    public class AdminController : Controller
    {
        private readonly IService service;

        public AdminController(IService service)
        {
            this.service = service;
        }

        [HttpPost]
        public HttpStatusCodeResult ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.service.ChangeRegistration(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpStatusCodeResult ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.service.ChangeUsersLimit(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public JsonResult ConfigurationStatus()
        {
            var configurationStatus = this.service.ReadConfiguration();
            return this.Json(configurationStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult Index()
        {
            return this.View("Index");
        }

        [HttpPost]
        public HttpStatusCodeResult SpinUpdateJob()
        {
            Task.Run(() => this.service.UpdateGlobalRss());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}