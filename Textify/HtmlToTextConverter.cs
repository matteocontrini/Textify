using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Textify
{
    public class HtmlToTextConverter
    {
        private string Convert(IHtmlElement element)
        {
            HtmlTraversal traversal = new HtmlTraversal();

            traversal.Traverse(element);

            return traversal
                .GetString()
                .Trim()
                // remove space at the end of each line
                .Replace(" \n", "\n");
        }

        public string Convert(string html)
        {
            HtmlParser parser = new HtmlParser();
            IHtmlDocument doc = parser.ParseDocument(html);

            return Convert(doc.Body);
        }
    }
}
