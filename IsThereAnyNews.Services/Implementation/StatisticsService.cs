using System.Collections.Generic;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;

namespace IsThereAnyNews.Services.Implementation
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        public TopReadChannelsViewModel GetTopReadChannels(int i)
        {
            var models = this.statisticsRepository.GetToReadChannels(i);
            var viewModels = models.Select(ProjectToViewModel).ToList();
            var viewmodel = new TopReadChannelsViewModel(viewModels);
            return viewmodel;
        }

        private ChannelWithSubscriptionCountViewModel ProjectToViewModel(ChannelWithSubscriptionCount model)
        {
            var viewModel = new ChannelWithSubscriptionCountViewModel
            {
                Id = model.Id,
                Title = model.Title,
                SubscriptionCount = model.SubscriptionCount
            };

            return viewModel;
        }

        public string GetUsersThatReadTheMost(int i)
        {
            this.statisticsRepository.GetUsersThatReadMostNews(i);
            return string.Empty;
        }

        public string GetTopReadNews(int i)
        {
            this.statisticsRepository.GetNewsThatWasReadMost(i);
            return string.Empty;
        }
    }
}