using System.Collections.Generic;

namespace IsThereAnyNews.Mvc.Controllers
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