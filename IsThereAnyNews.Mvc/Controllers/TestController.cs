namespace IsThereAnyNews.Mvc.Controllers
{
    using System.Web.Mvc;

    using IsThereAnyNews.Services;

    public class TestController : Controller
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        public ViewResult Index()
        {
            return this.View("Index");
        }

        [HttpGet]
        public ActionResult GenerateUsers()
        {
            this.testService.GenerateUsers();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DuplicateChannels()
        {
            this.testService.DuplicateChannels();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CreateSubscriptions()
        {
            this.testService.CreateSubscriptions();
            return this.RedirectToAction("Index");
        }

        public ActionResult CreateRssToRead()
        {
            this.testService.CreateRssToRead();
            return this.RedirectToAction("Index");
        }

        public ActionResult CreateRssViewedEvent()
        {
            this.testService.CreateRssViewedEvent();
            return this.RedirectToAction("Index");
        }

        public ActionResult FixSubscriptions()
        {
            this.testService.FixSubscriptions();
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AssignUserRoles()
        {
            this.testService.AssignUserRolesToAllUsers();
            return this.RedirectToAction("Index");
        }
    }
}