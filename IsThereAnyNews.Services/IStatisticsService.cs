using System.Collections.Generic;
using IsThereAnyNews.Services.Implementation;

namespace IsThereAnyNews.Services
{
    public interface IStatisticsService
    {
        TopReadChannelsViewModel GetTopReadChannels(int i);
        string GetUsersThatReadTheMost(int i);
        string GetTopReadNews(int i);
    }
}