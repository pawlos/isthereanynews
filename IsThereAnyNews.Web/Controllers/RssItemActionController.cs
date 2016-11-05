namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.Services;

    [Authorize]
    public class RssItemActionController : Controller
    {
        private readonly IRssItemActionService rssItemActionService;

        public RssItemActionController(IRssItemActionService rssItemActionService) 
        {
            this.rssItemActionService = rssItemActionService;
        }

        [HttpPost]
        public ActionResult Voteup(RssActionModel model)
        {
            this.rssItemActionService.CurrentVoteupForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult VoteDown(RssActionModel model)
        {
            this.rssItemActionService.CurrentVotedownForArticleByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkNotRead(RssActionModel model)
        {
            this.rssItemActionService.MarkRssItemAsNotReadByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Share(RssActionModel model)
        {
            this.rssItemActionService.ShareRssItem(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddComment(RssActionModel model)
        {
            this.rssItemActionService.AddCommentToRssItemByCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddToReadLater(RssActionModel model)
        {
            this.rssItemActionService.AddToReadLaterQueueForCurrentUser(model);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}