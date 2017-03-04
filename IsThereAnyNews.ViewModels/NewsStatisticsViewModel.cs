namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class NewsStatisticsViewModel
    {
        public NewsStatisticsViewModel(List<RssStatisticsViewModel> list)
        {
            this.List = list;
        }

        public List<RssStatisticsViewModel> List { get; set; }
    }
}