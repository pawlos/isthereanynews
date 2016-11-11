namespace IsThereAnyNews.HtmlStrip
{
    using System.Text;

    using HtmlAgilityPack;

    public class HtmlStripper : IHtmlStripper
    {
        public string GetContentOnly(string itemSummary)
        {
            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(itemSummary);
            StringBuilder content = new StringBuilder();

            foreach (var node in htmlDocument.DocumentNode.SelectNodes("//text()"))
            {
                content.AppendLine(node.InnerText);
            }

            return content.ToString().Replace("\n", "<br/>");
        }
    }
}