using AngleSharp.Dom;
using System.Text;
using System.Text.RegularExpressions;

namespace Textify
{
    internal class HtmlTraversal
    {
        private StringBuilder output;
        private bool justClosedDiv;
        private int lineLength;
        private int newLinesCount;
        private bool lastWasSpace;

        public HtmlTraversal()
        {
            this.output = new StringBuilder();
        }

        public string GetString()
        {
            return output.ToString();
        }

        public void Traverse(INode node)
        {
            switch (node.NodeType)
            {
                case NodeType.Element:
                    HandleElement((IElement)node);
                    break;

                case NodeType.Text:
                    string text = Regex.Replace(node.TextContent, "[ \r\n\t]{2,}", " ");
                    Write(text);
                    break;
                
                default:
                    TraverseChildren(node);
                    break;
            }
        }

        private void TraverseChildren(INode node)
        {
            foreach (INode child in node.ChildNodes)
            {
                Traverse(child);
            }
        }

        private void HandleElement(IElement element)
        {
            this.justClosedDiv = false;

            switch (element.TagName.ToUpper())
            {
                case "BR":
                    Write("\n");
                    break;

                case "H1":
                case "H2":
                case "H3":
                case "H4":
                case "H5":
                case "H6":
                    Write("\n\n");
                    TraverseChildren(element);
                    Write("\n\n");
                    break;

                case "DIV":
                    if (this.lineLength > 0)
                    {
                        Write("\n");
                    }

                    TraverseChildren(element);

                    if (!this.justClosedDiv) // avoid writing new lines twice
                    {
                        Write("\n");
                    }

                    this.justClosedDiv = true;

                    break;

                case "LI":
                    HtmlTraversal trav = new HtmlTraversal();
                    trav.TraverseChildren(element);

                    Write("* ");
                    Write(trav.GetString().Trim());
                    Write("\n");

                    break;

                case "P":
                case "UL":
                    Write("\n\n");
                    TraverseChildren(element);
                    Write("\n\n");
                    break;

                case "IMG":
                    string altText = element.GetAttribute("alt");

                    if (!string.IsNullOrWhiteSpace(altText))
                    {
                        Write("[IMG: ");
                        Write(altText);
                        Write("] ");
                    }

                    break;
                    
                case "STYLE":
                case "SCRIPT":
                case "HEAD":
                    break;

                default:
                    TraverseChildren(element);
                    break;
            }
        }

        private void Write(string text)
        {
            if (text == "")
            {
                return;
            }

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    // Write no more than 1 empty line (\n twice)
                    if (this.newLinesCount < 2)
                    {
                        this.output.Append(c);
                        this.lineLength = 0;
                        this.newLinesCount++;
                    }
                }
                else
                {
                    if (this.newLinesCount > 0)
                    {
                        // Skip empty characters at the beginning of lines
                        if (char.IsWhiteSpace(c))
                        {
                            continue;
                        }
                        else
                        {
                            this.newLinesCount = 0;
                        }
                    }

                    bool isSpace = char.IsWhiteSpace(c);

                    // Avoid writing 2 consecutive spaces
                    if (this.lastWasSpace && isSpace)
                    {
                        continue;
                    }

                    this.output.Append(c);
                    this.lineLength++;
                    this.lastWasSpace = isSpace;
                }
            }
        }
    }
}
