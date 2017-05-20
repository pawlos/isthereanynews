namespace IsThereAnyNews.HtmlStrip
{
    public interface IHtmlStripper
    {
        string GetContentOnly(string itemSummary);
    }
}