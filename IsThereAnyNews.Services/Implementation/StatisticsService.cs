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

        private UserWithStatisticsViewModel ProjectToViewModel(UserWithStatistics model)
        {
            var viewModel = new UserWithStatisticsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Count = model.Count
            };

            return viewModel;
        }

        private RssStatisticsViewModel ProjectToViewModel(RssStatistics model)
        {
            var viewModel = new RssStatisticsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Count = model.Count,
                Preview=model.Preview
            };

            return viewModel;
        }

        public UserStatisticsViewModel GetUsersThatReadTheMost(int i)
        {
            var models = this.statisticsRepository.GetUsersThatReadMostNews(i);
            var list = models.Select(ProjectToViewModel)
                .ToList();
            var userStatisticsViewModel = new UserStatisticsViewModel(list);
            return userStatisticsViewModel;
        }

        public NewsStatisticsViewModel GetTopReadNews(int i)
        {
            var models = this.statisticsRepository.GetNewsThatWasReadMost(i);
            var list = models.Select(ProjectToViewModel)
                .ToList();
            var userStatisticsViewModel = new NewsStatisticsViewModel(list);
            return userStatisticsViewModel;
        }
    }
}