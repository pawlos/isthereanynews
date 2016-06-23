using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using IsThereAnyNews.DataAccess;
using IsThereAnyNews.DataAccess.Implementation;
using IsThereAnyNews.EntityFramework.Models.Events;
using IsThereAnyNews.ViewModels;

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
                Preview = model.Preview
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

        public List<ActivityPerWeek> GetActivityPerWeek()
        {
            var startDate = new DateTime(2016, 1, 1);
            var endDate = DateTime.Now.Date;

            var loadAllEventsFromAndToDate =
                this.statisticsRepository
                    .LoadAllEventsFromAndToDate(startDate, endDate);

            var getWeekOfYear = new Func<DateTime, CalendarWeekRule, DayOfWeek, int>(CultureInfo.CurrentCulture.Calendar.GetWeekOfYear);

            var x = new List<List<EventRssViewed>>(52);
            for (int i = 0; i < 52; i++)
            {
                x.Add(new List<EventRssViewed>());
            }

            loadAllEventsFromAndToDate.ForEach(e =>
                x.ElementAt(getWeekOfYear(e.Created,
                            CalendarWeekRule.FirstFourDayWeek,
                                DayOfWeek.Monday)).Add(e));

            var r = new List<ActivityPerWeek>(52);
            for (int i = 1; i <= 52; i++)
            {
                var eventRssVieweds = x.ElementAt(i - 1);
                var rssVieweds = eventRssVieweds
                    .GroupBy(e => e.RssEntryId)
                    .OrderByDescending(e => e.Key)
                    .Take(3)
                    .SelectMany(e => e.Take(3))
                    .ToList();

                r.Add(new ActivityPerWeek(i, eventRssVieweds.Count, rssVieweds));
            }
            return r;
        }
    }

    public class ActivityPerWeek
    {
        public int WeekNumber { get; set; }
        public int RssCount { get; set; }
        public List<EventRssViewed> RssVieweds { get; set; }

        public ActivityPerWeek(int weekNumber, int rssCount, List<EventRssViewed> rssVieweds)
        {
            WeekNumber = weekNumber;
            RssCount = rssCount;
            RssVieweds = rssVieweds;
        }
    }
}