namespace IsThereAnyNews.Automapper
{
    using System.ServiceModel.Syndication;

    using AutoMapper;

    using IsThereAnyNews.Services.Implementation;

    public class SyndicationToAdapter : Profile
    {
        public SyndicationToAdapter()
        {
            this.CreateMap<SyndicationItem, SyndicationItemAdapter>();
        }
    }
}