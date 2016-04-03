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

    public class NewsStatisticsViewModel
    {
        public List<RssStatisticsViewModel> List { get; set; }

        public NewsStatisticsViewModel(List<RssStatisticsViewModel> list)
        {
            List = list;
        }
    }

    public class RssStatisticsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Preview { get; set; }
    }

   

    public class UserStatisticsViewModel
    {
        public List<UserWithStatisticsViewModel> List { get; set; }

        public UserStatisticsViewModel(List<UserWithStatisticsViewModel> list)
        {
            List = list;
        }
    }

    public class UserWithStatisticsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}