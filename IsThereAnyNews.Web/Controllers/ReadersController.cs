namespace IsThereAnyNews.Web.Controllers
{
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class ReadersController: Controller
    {
        private readonly IService service;

        public ReadersController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            var usersPublicProfileViewModel = this.service.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }

        public virtual ActionResult Account(long id)
        {
            var userPublicProfile = this.service.LoadUserPublicProfile(id);
            return this.View("User", userPublicProfile);
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult SubscribeToUser(SubscribeToUserActivityDto model)
        {
            this.service.SubscribeToUser(model);
            return this.RedirectToAction(MVC.Readers.Account(model.ViewingUserId));
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult UnsubscribeFromUser(SubscribeToUserActivityDto model)
        {
            this.service.UnsubscribeToUser(model);
            return this.RedirectToAction(MVC.Readers.Account(model.ViewingUserId));
        }
    }
}