using System.Net;
using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    [Authorize]
    public class RssItemActionController : Controller
    {
        private readonly IRssItemActionService rssItemActionService;

        public RssItemActionController(IRssItemActionService rssItemActionService) 
        {
            this.rssItemActionService = rssItemActionService;
        }

        [HttpPost]
        public ActionResult Voteup(long id)
        {
            this.rssItemActionService.CurrentVoteupForArticleByCurrentUser(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult VoteDown(long id)
        {
            this.rssItemActionService.CurrentVotedownForArticleByCurrentUser(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult MarkNotRead(long id)
        {
            this.rssItemActionService.MarkRssItemAsNotReadByCurrentUser(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult Share(long id)
        {
            this.rssItemActionService.ShareRssItem(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddComment(long id)
        {
            this.rssItemActionService.AddCommentToRssItemByCurrentUser(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult OpenFull(long id)
        {
            this.rssItemActionService.OpenFullArticle(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult AddToReadLater(long id)
        {
            this.rssItemActionService.AddToReadLaterQueueForCurrentUser(id);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}