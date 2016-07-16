namespace IsThereAnyNews.Automapper
{
    using AutoMapper;

    using IsThereAnyNews.Dtos;
    using IsThereAnyNews.EntityFramework.Models.Entities;

    public class DtoToEntityModelProfile : Profile
    {
        public DtoToEntityModelProfile()
        {
            this.CreateMap<AddChannelDto, RssChannel>()
                .ForMember(d => d.Title, o => o.MapFrom(s => s.RssChannelName))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.RssChannelLink))
                .ForMember(d => d.Created, o => o.Ignore())
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.RssEntries, o => o.Ignore())
                .ForMember(d => d.RssLastUpdatedTime, o => o.Ignore())
                .ForMember(d => d.Subscriptions, o => o.Ignore())
                .ForMember(d => d.Updated, o => o.Ignore());
        }
    }
}