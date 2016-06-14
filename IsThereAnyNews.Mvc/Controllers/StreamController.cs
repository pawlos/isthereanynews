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
           ISessionProvider sessionProvider,
           IRssSubscriptionService rssSubscriptionService)
           : base(authentication, loginService, sessionProvider)
        {
            this.rssSubscriptionService = rssSubscriptionService;
        }

        [HttpGet]
        public ActionResult Read(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.rssSubscriptionService
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            return this.View("Read", entries);
        }

        [HttpGet]
        public ActionResult ReadAjax(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.rssSubscriptionService
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            return this.PartialView("_ReadAjax", entries);
        }

        [HttpPost]
        public ActionResult MarkRead(MarkReadDto dto)
        {
            this.rssSubscriptionService.MarkRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkReadWithEvent(MarkReadDto dto)
        {
            this.rssSubscriptionService.MarkRead(dto);
            this.rssSubscriptionService.MarkEntryViewed(long.Parse(dto.DisplayedItems));
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkClicked(MarkClickedDto dto)
        {
            this.rssSubscriptionService.MarkEntryClicked(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}