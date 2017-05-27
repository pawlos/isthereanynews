namespace IsThereAnyNews.Automapper
{
    using System;
    using System.Linq;
    using System.ServiceModel.Syndication;

    using AutoMapper;

    using IsThereAnyNews.ProjectionModels.Mess;

    public class SyndicationUrlResolver : IValueResolver<SyndicationItem, SyndicationItemAdapter, string>
    {
        public string Resolve(
            SyndicationItem source,
            SyndicationItemAdapter destination,
            string destMember,
            ResolutionContext context)
        {
            if (source.Links != null && source.Links.Any(x=>x.RelationshipType == "alternate"))
            {
                return source.Links.First(x=>x.RelationshipType== "alternate").Uri.ToString();
            }

            if(source.BaseUri != null && !string.IsNullOrWhiteSpace(source.BaseUri.ToString()))
            {
                return source.BaseUri.ToString();
            }

            if(source.Links != null && source.Links.Any())
            {
                return source.Links.First().Uri.ToString();
            }


            throw new Exception("No link found");
        }
    }
}
