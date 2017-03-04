namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;
    using IsThereAnyNews.SharedData;

    [Authorize]
    public class StreamController : Controller
    {
        private readonly IService service;

        public StreamController(IService service)
        {
            this.service = service;
        }

        [HttpGet]
        public ActionResult ReadAjax(StreamType streamType, long id, ShowReadEntries showReadEntries = ShowReadEntries.Hide)
        {
            var entries = this.service
                .LoadAllUnreadRssEntriesToReadForCurrentUserFromSubscription(streamType, id, showReadEntries);
            var result = this.Json(entries, JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        public ActionResult MarkEntriesRead(MarkReadDto dto)
        {
            this.service.MarkEntriesRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkEntriesSkipped(MarkSkippedDto model)
        {
            this.service.MarkEntriesSkipped(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkReadWithEvent(MarkReadDto dto)
        {
            this.service.MarkRead(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkClickedWithEvent(MarkClickedDto dto)
        {
            this.service.MarkClicked(dto);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}