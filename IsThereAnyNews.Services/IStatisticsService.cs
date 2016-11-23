using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;
using System.Collections.Generic;

namespace IsThereAnyNews.Services
{
    public interface IStatisticsService
    {
        TopReadChannelsViewModel GetTopReadChannels(int i);

        UserStatisticsViewModel GetUsersThatReadTheMost(int i);

        NewsStatisticsViewModel GetTopReadNews(int i);

        List<ActivityPerWeek> GetActivityPerWeek();
    }
}