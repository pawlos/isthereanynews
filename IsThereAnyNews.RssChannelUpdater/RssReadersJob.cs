using FluentScheduler;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.RssChannelUpdater
{
    public class RssReadersJob : IJob
    {
        private readonly ITestService testService;

        public RssReadersJob(ITestService testService)
        {
            this.testService = testService;
        }

        public void Execute()
        {
            this.testService.ReadRandomRssEvent();
        }
    }
}