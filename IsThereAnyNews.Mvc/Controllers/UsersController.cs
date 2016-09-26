namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService,
                               IUserAuthentication authentication,
                               ILoginService loginService, 
                               ISessionProvider sessionProvider)
            : base(authentication, loginService, sessionProvider)
        {
            this.usersService = usersService;
        }

        [AllowAnonymous]
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
            this.usersService.SubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }

        public ActionResult Unsubscribe(SubscribeToUserActivityDto model)
        {
            this.usersService.UnsubscribeToUser(model);
            return this.RedirectToAction("Profile", new { id = model.ViewingUserId });
        }
    }
}