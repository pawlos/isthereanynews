namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;

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
        public ActionResult ReadAjax(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.rssSubscriptionService
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            var result = this.Json(entries, JsonRequestBehavior.AllowGet);
            return result;
        } 

        [HttpPost]
        public ActionResult MarkReadWithEvent(MarkReadDto dto)
        {
            this.rssSubscriptionService.MarkRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkClickedWithEvent(MarkClickedDto dto)
        {
            this.rssSubscriptionService.MarkClicked(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}