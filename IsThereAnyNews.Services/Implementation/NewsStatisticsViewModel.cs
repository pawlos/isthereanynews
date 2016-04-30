using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
{
    public class NewsStatisticsViewModel
    {
        public List<RssStatisticsViewModel> List { get; set; }

        public NewsStatisticsViewModel(List<RssStatisticsViewModel> list)
        {
            List = list;
        }
    }
}