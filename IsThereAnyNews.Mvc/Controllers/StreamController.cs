using System.Web.Mvc;
using IsThereAnyNews.Dtos;
using IsThereAnyNews.Services;
using IsThereAnyNews.SharedData;

namespace IsThereAnyNews.Mvc.Controllers
{
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

        public ActionResult Read(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.rssSubscriptionService
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            return this.View("Index", entries);
        }

        public ActionResult MarkRead(MarkReadDto dto)
        {
            this.rssSubscriptionService.MarkRead(dto);
            return this.RedirectToAction("Read", new { StreamType = dto.StreamType, id = dto.Id });
        }
    }
}