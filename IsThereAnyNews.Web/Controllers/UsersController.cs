namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    public class UsersController : Controller
    {
        private readonly IService service;

        public UsersController(IService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            var usersPublicProfileViewModel = this.service.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Profile(long id)
        {
            var userPublicProfile = this.service.LoadUserPublicProfile(id);
            return this.View("Profile", userPublicProfile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Subscribe(SubscribeToUserActivityDto model)
        {
            this.service.SubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Unsubscribe(SubscribeToUserActivityDto model)
        {
            this.service.UnsubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }
    }
}