namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService,
                               IUserAuthentication authentication,
                               ILoginService loginService)
            : base(authentication, loginService)
        {
            this.usersService = usersService;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            var usersPublicProfileViewModel = this.usersService.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Profile(long id)
        {
            var userPublicProfile = this.usersService.LoadUserPublicProfile(id);
            return this.View("Profile", userPublicProfile);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Subscribe(SubscribeToUserActivityDto model)
        {
            this.usersService.SubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Unsubscribe(SubscribeToUserActivityDto model)
        {
            this.usersService.UnsubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }
    }
}