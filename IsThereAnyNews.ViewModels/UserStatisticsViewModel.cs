namespace IsThereAnyNews.ViewModels
{
    using System.Collections.Generic;

    public class UserStatisticsViewModel
    {
        public UserStatisticsViewModel(List<UserWithStatisticsViewModel> list)
        {
            this.List = list;
        }

        public List<UserWithStatisticsViewModel> List { get; set; }
    }
}