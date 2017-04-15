namespace IsThereAnyNews.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.SharedData;
    using IsThereAnyNews.ViewModels;
    using IsThereAnyNews.ViewModels.Login;
    using IsThereAnyNews.Web.Infrastructure;
    using IsThereAnyNews.Web.Interfaces.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    [AllowAnonymous]
    public partial class HomeController: Controller
    {
        private readonly IService service;
        private const string XsrfKey = "XsrfId";

        public HomeController(IService service)
        {
            this.service = service;
        }
        public virtual ActionResult Index()
        {
            return this.View("Index");
        }

        

        [HttpPost]
        public virtual ActionResult MarkClickedWithEvent(MarkClickedDto dto)
        {
            this.service.MarkClicked(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult MarkEntriesRead(MarkReadDto dto)
        {
            this.service.MarkEntriesRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult MarkEntriesSkipped(MarkSkippedDto model)
        {
            this.service.MarkEntriesSkipped(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult MarkReadWithEvent(MarkReadDto dto)
        {
            this.service.MarkRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public virtual ActionResult ReadAjax(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.service.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            var result = this.Json(entries, JsonRequestBehavior.AllowGet);
            return result;
        }

        public virtual ActionResult Logout()
        {
            this.Session.Abandon();
            this.Session.Clear();
            this.HttpContext
                .GetOwinContext()
                .Authentication
                .SignOut(ConstantStrings.AuthorizationCookieName);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public virtual ActionResult ImportOpml()
        {
            var viewmodel = new OpmlImporterIndexViewModel();
            return this.View("ImportOpml", viewmodel);
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult ImportOpml(OpmlImporterIndexDto dto)
        {
            if(!this.ModelState.IsValid)
            {
                return this.View("Index", null);
            }

            this.service.Import(dto);
            return this.RedirectToAction("My", "Home");
        }

        [HttpGet]
        public virtual ActionResult Users()
        {
            var usersPublicProfileViewModel = this.service.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }

        [HttpGet]
        public virtual ActionResult User(long id)
        {
            var userPublicProfile = this.service.LoadUserPublicProfile(id);
            return this.View("User", userPublicProfile);
        }

        [HttpPost]
        [Authorize]
        public virtual ActionResult SubscribeToUser(SubscribeToUserActivityDto model)
        {
            this.service.SubscribeToUser(model);
            return this.RedirectToAction("User", new { id = model.ViewingUserId });
        }

        [Authorize]
        [HttpPost]
        public virtual ActionResult UnsubscribeFromUser(SubscribeToUserActivityDto model)
        {
            this.service.UnsubscribeToUser(model);
            return this.RedirectToAction("User", new { id = model.ViewingUserId });
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
        public virtual ViewResult Admin()
        {
            return this.View("Admin");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.SuperAdmin })]
        public virtual HttpStatusCodeResult SpinUpdateJob()
        {
            Task.Run(() => this.service.UpdateGlobalRss());
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        private IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext()
            .Authentication;

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Home", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if(loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            var identity = new ClaimsIdentity(loginInfo.ExternalIdentity.Claims, DefaultAuthenticationTypes.ApplicationCookie);
            var currentRegistrationStatus = this.service.GetCurrentRegistrationStatus();
            switch(currentRegistrationStatus)
            {
                case RegistrationSupported.Closed:
                if(this.service.IsUserRegistered(identity) == false)
                {
                    return this.RedirectToAction("RegistrationClosed");
                }

                break;
                case RegistrationSupported.Open:
                // nothing here, proceed with regular registration
                break;
                case RegistrationSupported.InviteOnly:
                // not supported yet
                break;
                case RegistrationSupported.Limited:
                if(this.service.IsUserRegistered(identity) == false
                    && this.service.CanRegisterIfWithinLimits() == false)
                {
                    return this.RedirectToAction("RegistrationClosed");
                }

                break;
                default:
                throw new ArgumentOutOfRangeException();
            }

            this.service.RegisterIfNewUser(identity);
            this.service.StoreCurrentUserIdInSession(identity);
            this.service.StoreItanRolesToSession(identity);
            this.AuthenticationManager.SignIn(identity);
            return this.RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        [AllowAnonymous]
        public virtual ActionResult Login(string returnUrl)
        {
            var viewmodel = new AuthorizationIndexViewModel();
            var providers = this.HttpContext.GetOwinContext()
                .Authentication.GetAuthenticationTypes(x => !string.IsNullOrWhiteSpace(x.Caption))
                .ToList();
            var currentRegistrationStatus = this.service.GetCurrentRegistrationStatus();
            viewmodel.Providers = providers;
            viewmodel.CurrentRegistrationStatus = currentRegistrationStatus.ToString();
            return this.View("Login", viewmodel);
        }

        public virtual ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public virtual ActionResult RegistrationClosed()
        {
            this.Session.Clear();
            this.Request.GetOwinContext()
                .Authentication.SignOut(
                    this.HttpContext.GetOwinContext()
                        .Authentication.GetAuthenticationTypes()
                        .Select(o => o.AuthenticationType)
                        .ToArray());
            return this.View("RegistrationClosed");
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult AddChannel()
        {
            return this.View("AddChannel");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult AddChannel(AddChannelDto dto)
        {
            if(this.ModelState.IsValid == false)
            {
                return this.View("AddChannel", dto);
            }

            this.service.CreateNewChannelIfNotExists(dto);
            this.service.SubscribeCurrentUserToChannel(dto);
            return this.RedirectToAction("My");
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.service.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual HttpStatusCodeResult MarkRssEntryViewed(long channelId)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual ActionResult My()
        {
            return this.View("My");
        }

        [HttpGet]
        [RoleAuthorize(Roles = new[] { ItanRole.User })]
        public virtual JsonResult MyChannelList()
        {
            var viewmodel = this.service.LoadAllChannelsOfCurrentUser();
            var listOfUsers = this.service.LoadAllObservableSubscription();
            var events = this.service.LoadEvents();
            viewmodel.Users = listOfUsers;
            viewmodel.Events = events;
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual ActionResult Public(long id)
        {
            var viewmodel = this.service.GetViewModelFormChannelId(id);
            return this.Json(viewmodel, JsonRequestBehavior.AllowGet);
        }

        



        [HttpPost]
        public virtual ActionResult AddComment(RssActionModel model)
        {
            this.service.AddCommentToRssItemByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult AddToReadLater(RssActionModel model)
        {
            this.service.AddToReadLaterQueueForCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult MarkNotRead(RssActionModel model)
        {
            this.service.MarkRssItemAsNotReadByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult Share(RssActionModel model)
        {
            this.service.ShareRssItem(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult VoteDown(RssActionModel model)
        {
            this.service.CurrentVotedownForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public virtual ActionResult Voteup(RssActionModel model)
        {
            this.service.CurrentVoteupForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpGet]
        public virtual ActionResult ChangeDisplayName()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpPost]
        public virtual ActionResult ChangeDisplayName(ChangeDisplayNameModelDto model)
        {
            if(this.ModelState.IsValid)
            {
                this.service.ChangeDisplayName(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeDisplayName", viewmodel);
        }

        [HttpGet]
        public virtual ActionResult ChangeEmail()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }

        [HttpPost]
        public virtual ActionResult ChangeEmail(ChangeEmailModelDto model)
        {
            if(this.ModelState.IsValid)
            {
                this.service.ChangeEmail(model);
                return this.RedirectToAction("Index");
            }

            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("ChangeEmail", viewmodel);
        }

        [HttpGet]
        public virtual ActionResult Account()
        {
            var viewmodel = this.service.GetAccountDetailsViewModel();
            return this.View("Account", viewmodel);
        }


        internal class ChallengeResult: HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null) { }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = this.RedirectUri };
                if(this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }

                context.HttpContext.GetOwinContext()
                    .Authentication.Challenge(properties, this.LoginProvider);
            }
        }
    }
}