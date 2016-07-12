namespace IsThereAnyNews.Services.Implementation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Xml;

    using AutoMapper;

    public class SyndicationFeedAdapter : ISyndicationFeedAdapter
    {
        private readonly IMapper mapper;

        public SyndicationFeedAdapter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public List<SyndicationItemAdapter> Load(string url)
        {
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            var syndicationItems = feed.Items.ToList();
            var items = this.mapper.Map<List<SyndicationItemAdapter>>(syndicationItems);
            return items;
        }
    }
}