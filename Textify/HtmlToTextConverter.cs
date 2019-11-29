using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Textify
{
    public class HtmlToTextConverter
    {
        public string Convert(INode node)
        {
            HtmlTraversal traversal = new HtmlTraversal();

            traversal.Traverse(node);

            return traversal.GetString().Trim();
        }

        public string Convert(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument doc = parser.ParseDocument(html);

            return Convert(doc.Body);
        }
    }
}
