using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels
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