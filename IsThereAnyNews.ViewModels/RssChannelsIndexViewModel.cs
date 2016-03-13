using System.Collections.Generic;
using IsThereAnyNews.EntityFramework.Models;

namespace IsThereAnyNews.ViewModels
{
    public class RssChannelsIndexViewModel
    {
        public List<RssChannel> LoadAllChannels { get; set; }

        public RssChannelsIndexViewModel(List<RssChannel> loadAllChannels)
        {
            LoadAllChannels = loadAllChannels;
        }
    }
}