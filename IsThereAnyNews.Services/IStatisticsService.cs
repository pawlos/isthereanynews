using System.Collections.Generic;
using IsThereAnyNews.Services.Implementation;
using IsThereAnyNews.ViewModels;

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