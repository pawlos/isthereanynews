using System.Collections.Generic;

namespace IsThereAnyNews.ViewModels
{
    public class UserStatisticsViewModel
    {
        public List<UserWithStatisticsViewModel> List { get; set; }

        public UserStatisticsViewModel(List<UserWithStatisticsViewModel> list)
        {
            List = list;
        }
    }
}