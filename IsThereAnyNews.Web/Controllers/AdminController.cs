namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class AdminController: Controller
    {
        private readonly IService service;

        public AdminController(IService service)
        {
            this.service = service;
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual HttpStatusCodeResult ChangeRegistration(ChangeRegistrationDto dto)
        {
            this.service.ChangeRegistration(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual HttpStatusCodeResult ChangeUsersLimit(ChangeUsersLimitDto dto)
        {
            this.service.ChangeUsersLimit(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual JsonResult ConfigurationStatus()
        {
            var configurationStatus = this.service.ReadConfiguration();
            return this.Json(configurationStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual ViewResult Index()
        {
            return this.View("Index");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual HttpStatusCodeResult SpinUpdateJob()
        {
            Task.Run(() => this.service.UpdateGlobalRss());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}