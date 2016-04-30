using System.Collections.Generic;

namespace IsThereAnyNews.Services.Implementation
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