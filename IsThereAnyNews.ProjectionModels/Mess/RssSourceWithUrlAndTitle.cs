namespace IsThereAnyNews.ProjectionModels.Mess
{
    public class RssSourceWithUrlAndTitle
    {
        public RssSourceWithUrlAndTitle(string url, string title)
        {
            this.Url = url;
            this.Title = title;
        }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}