namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Web.Interfaces.Services;

    public partial class EntriesController: Controller
    {
        private readonly IService service;

        public EntriesController(IService service)
        {
            this.service = service;
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
    }
}