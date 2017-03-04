namespace IsThereAnyNews.Automapper
{
    using System.ServiceModel.Syndication;

    using AutoMapper;

    using IsThereAnyNews.ProjectionModels.Mess;

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