﻿using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    [AllowAnonymous]
    public class RssChannelController : BaseController
    {
        private readonly IRssChannelsService rssChannelsService;
        private readonly IRssSubscriptionService rssSubscriptionService;
        private readonly IUserSubscriptionService userSubscriptionServiceService;

        public RssChannelController(
            IUserAuthentication authentication,
            ILoginService loginService,
            IRssChannelsService rssChannelsService,
            IRssSubscriptionService rssSubscriptionService, 
            IUserSubscriptionService userSubscriptionServiceService)
            : base(authentication, loginService)
        {
            this.rssChannelsService = rssChannelsService;
            this.rssSubscriptionService = rssSubscriptionService;
            this.userSubscriptionServiceService = userSubscriptionServiceService;
        }

        [HttpGet]
        public ActionResult Public(long id)
        {
            var viewmodel = this.rssChannelsService.GetViewModelFormChannelId(id);
            return this.PartialView("_Public", viewmodel);
        }

        public ActionResult Index()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannels();
            return this.View("Index", viewmodel);
        }

        [Authorize]
        public ActionResult My()
        {
            var viewmodel = this.rssChannelsService.LoadAllChannelsOfCurrentUser();
            var listofusers = this.userSubscriptionServiceService.LoadAllObservableSubscription();
            viewmodel.Users = listofusers;
            return this.View("My", viewmodel);
        }

        [Authorize]
        [HttpPost]
        public HttpStatusCodeResult Unsubscribe(long id)
        {
            this.rssSubscriptionService.UnsubscribeCurrentUserFromChannelId(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Subscribe(long id)
        {
            this.rssSubscriptionService.SubscribeCurrentUserToChannel(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public PartialViewResult Details(long id)
        {
            var entries = this.rssSubscriptionService.LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(id);
            return PartialView("_Details", entries);
        }

        public HttpStatusCodeResult MarkRssEntryViewed(long id)
        {
            this.rssSubscriptionService.MarkEntryViewed(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult MarkAllReadForSubscription(MarkReadForSubscriptionDto model)
        {
            this.rssSubscriptionService.MarkAllRssReadForSubscription(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}