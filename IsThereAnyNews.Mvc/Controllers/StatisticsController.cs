using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        public ActionResult Index()
        {
            return this.View("Index");
        }

        public ActionResult Channels()
        {
            var viewmodel = this.statisticsService.GetTopReadChannels(10);
            return this.View("Channels", viewmodel);
        }

        public ActionResult News()
        {
            var viewmodel = this.statisticsService.GetTopReadNews(10);
            return this.View("News", viewmodel);
        }

        public ActionResult Users()
        {
            var viewmodel = this.statisticsService.GetUsersThatReadTheMost(10);
            return this.View("Users", viewmodel);
        }


        public ActionResult ActivityPerWeek()
        {
            var activityPerWeeks = this.statisticsService.GetActivityPerWeek();
            return this.View("ActivityPerWeek", activityPerWeeks);
        }
    }
}