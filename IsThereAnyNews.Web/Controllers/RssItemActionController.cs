namespace IsThereAnyNews.Web.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    [Authorize]
    public class RssItemActionController : Controller
    {
        private readonly IService service;

        public RssItemActionController(IService service)
        {
            this.service = service;
        }

        [HttpPost]
        public ActionResult Voteup(RssActionModel model)
        {
            this.service.CurrentVoteupForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult VoteDown(RssActionModel model)
        {
            this.service.CurrentVotedownForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkNotRead(RssActionModel model)
        {
            this.service.MarkRssItemAsNotReadByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Share(RssActionModel model)
        {
            this.service.ShareRssItem(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddComment(RssActionModel model)
        {
            this.service.AddCommentToRssItemByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddToReadLater(RssActionModel model)
        {
            this.service.AddToReadLaterQueueForCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}