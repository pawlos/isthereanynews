using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public ActionResult Index()
        {
            var usersPublicProfileViewModel = this.usersService.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }

        public ActionResult Profile(long id)
        {
            var userPublicProfile = this.usersService.LoadUserPublicProfile(id);
            return this.View("Profile", userPublicProfile);
        }

        [HttpPost]
        public ActionResult Subscribe(SubscribeToUserActivityDto model)
        {
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }
    }

    public class SubscribeToUserActivityDto
    {
        public long ViewingUserId { get; set; }
    }
}