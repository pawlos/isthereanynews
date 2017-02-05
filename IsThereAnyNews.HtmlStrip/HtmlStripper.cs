namespace IsThereAnyNews.HtmlStrip
{
    using System;
    using System.Text;

    using HtmlAgilityPack;

    public class HtmlStripper : IHtmlStripper
    {
        public string GetContentOnly(string itemSummary)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(itemSummary);
            StringBuilder content = new StringBuilder();

            try
            {
                foreach (var node in htmlDocument.DocumentNode.SelectNodes("//text()"))
                {
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                    {
                        content.AppendLine(node.InnerText);
                    }
                }
                return content.ToString().Replace("\n", "<br/>");
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}