namespace IsThereAnyNews.HtmlStrip
{
    using System;

    public interface IHtmlStripper
    {
        string GetContentOnly(string itemSummary);
    }
}