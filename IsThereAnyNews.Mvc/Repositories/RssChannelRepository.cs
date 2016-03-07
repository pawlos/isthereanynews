namespace IsThereAnyNews.Mvc.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class RssChannelRepository
    {
        static private List<RssChannel> listOfChannels = new List<RssChannel>();

        public List<RssChannel> AddToGlobalSpace(List<RssChannel> importFromUpload)
        {
            var savedList = new List<RssChannel>();
            foreach (var channel in importFromUpload)
            {
                var rssChannel = new RssChannel(channel.Url, channel.Title)
                {
                    Id = CreateId()
                };

                listOfChannels.Add(rssChannel);
            }

            return savedList;
        }

        private static long CreateId()
        {
            return listOfChannels.Any() ? listOfChannels.Max(x => x.Id) + 1 : 0;
        }

        public List<RssChannel> LoadAllChannels()
        {
            return listOfChannels;
        }

        public RssChannel Load(long id)
        {
            return listOfChannels.Single(channel => channel.Id == id);
        }
    }
}