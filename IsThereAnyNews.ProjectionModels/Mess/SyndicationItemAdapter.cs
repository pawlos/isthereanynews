namespace IsThereAnyNews.ProjectionModels.Mess
{
    using System;

    public class SyndicationItemAdapter
    {
        public string Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
    }
}