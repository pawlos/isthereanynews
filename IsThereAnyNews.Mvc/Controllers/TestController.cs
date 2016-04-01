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
    }
}