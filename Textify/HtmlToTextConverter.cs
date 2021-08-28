using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Textify
{
    public class HtmlToTextConverter
    {
        public string Convert(INode node)
        {
            List<string> links = new List<string>();
            HtmlTraversal traversal = new HtmlTraversal(links);

            traversal.Traverse(node);

            return traversal.GetString(true).Trim();
        }

        public string Convert(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument doc = parser.ParseDocument(html);

            return Convert(doc.Body);
        }
    }
}
