namespace IsThereAnyNews.Automapper
{
    using System.ServiceModel.Syndication;

    using AutoMapper;

    using IsThereAnyNews.Services.Implementation;

    public class SyndicationToAdapter : Profile
    {
        public SyndicationToAdapter()
        {
            this.CreateMap<SyndicationItem, SyndicationItemAdapter>()
                .ForMember(s => s.Id, o => o.MapFrom(s => s.Id))
                .ForMember(s => s.PublishDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(s => s.Summary, o => o.MapFrom(s => s.Summary.Text))
                .ForMember(s => s.Title, o => o.MapFrom(s => s.Title.Text))
                .ForMember(s => s.Url, o => o.MapFrom(s => s.BaseUri.ToString()));
        }
    }
}