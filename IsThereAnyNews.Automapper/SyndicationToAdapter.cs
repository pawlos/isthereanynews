namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Threading;

    using AutoMapper;

    using IsThereAnyNews.Services.Implementation;

    public class SyndicationToAdapter : Profile
    {
        public SyndicationToAdapter()
        {
            this.CreateMap<SyndicationItem, SyndicationItemAdapter>()
                .ForMember(s => s.Id, o => o.MapFrom(s => s.Id))
                .ForMember(s => s.PublishDate, o => o.MapFrom(s => s.PublishDate))
                .ForMember(s => s.Summary, o => o.ResolveUsing<SyndicationSummaryResolver>())
                .ForMember(s => s.Title, o => o.MapFrom(s => s.Title.Text))
                .ForMember(s => s.Url, o => o.ResolveUsing<SyndicationUrlResolver>());
        }

        public class SyndicationUrlResolver : IValueResolver<SyndicationItem, SyndicationItemAdapter, string>
        {
            public string Resolve(
                SyndicationItem source,
                SyndicationItemAdapter destination,
                string destMember,
                ResolutionContext context)
            {
                if (source.BaseUri != null && !string.IsNullOrWhiteSpace(source.BaseUri.ToString()))
                {
                    return source.BaseUri.ToString();
                }

                if (source.Links != null && source.Links.Any())
                {
                    return source.Links.First().Uri.ToString();
                }

                throw new Exception("No link found");
            }
        }
    }

    public class SyndicationSummaryResolver : IValueResolver<SyndicationItem, SyndicationItemAdapter, string>
    {
        public string Resolve(SyndicationItem source, SyndicationItemAdapter destination, string destMember, ResolutionContext context)
        {
            var t = source.Content as TextSyndicationContent;
            if (t != null && !string.IsNullOrEmpty(t.Text))
            {
                return t.Text;
            }

            if (!string.IsNullOrEmpty(source.Summary.Text))
            {
                return source.Summary.Text;
            }

            return "dont know how to solve the missing summary issue";
            //throw new Exception("Summary is empty");
        }
    }
}