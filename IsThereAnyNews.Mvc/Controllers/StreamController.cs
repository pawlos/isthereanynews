using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Controllers
{
    [Authorize]
    public class StreamController : BaseController
    {
        private readonly IRssSubscriptionService rssSubscriptionService;

        public StreamController(
           IUserAuthentication authentication,
           ILoginService loginService,
           IRssSubscriptionService rssSubscriptionService)
           : base(authentication, loginService)
        {
            this.rssSubscriptionService = rssSubscriptionService;
        }

        [HttpGet]
        public ActionResult Read(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.rssSubscriptionService
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            return this.PartialView("_Read", entries);
        }

        [HttpPost]
        public ActionResult MarkRead(MarkReadDto dto)
        {
            this.rssSubscriptionService.MarkRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}