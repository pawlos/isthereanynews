namespace IsThereAnyNews.Automapper
{
    using System.ServiceModel.Syndication;

    using AutoMapper;

    using IsThereAnyNews.Services.Implementation;

    public class SyndicationToAdapter : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<SyndicationItem, SyndicationItemAdapter>();
        }
    }
}