using System.Collections.Generic;
using AngleSharp.Dom;
using System.Linq;
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
        private List<string> links;

        public HtmlTraversal()
        {
            this.links = new List<string>();
            this.output = new StringBuilder();
        }

        public string GetString()
        {
            if (links.Count() > 0)
            {
                Write("\n\n");
                for (var linkIndex = 0; linkIndex < links.Count(); linkIndex++)
                {
                    var link = links[linkIndex];
                    Write($"[{linkIndex + 1}] {link}{(linkIndex < links.Count() - 1 ? "\n" : string.Empty)}");
                }
            }

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
                    string text = Regex.Replace(node.TextContent, "[ \r\n\t]+", " ");
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

            string tagName = element.TagName.ToUpper();

            switch (tagName)
            {
                case "BR":
                    Write("\n");
                    break;

                case "H1":
                case "H2":
                case "H3":
                    Write("\n\n");

                    HtmlTraversal headingTrav = new HtmlTraversal();
                    headingTrav.TraverseChildren(element);

                    string headingText = headingTrav.GetString().Trim();

                    if (string.IsNullOrEmpty(headingText))
                    {
                        break;
                    }

                    int dividerLength = headingText.Split('\n').Max(x => x.Length);
                    string divider;

                    if (tagName == "H1")
                    {
                        divider = new string('+', dividerLength);
                    }
                    else
                    {
                        divider = new string('-', dividerLength);
                    }

                    if (tagName == "H3")
                    {
                        Write(headingText);
                        Write("\n");
                        Write(divider);
                    }
                    else
                    {
                        Write(divider);
                        Write("\n");
                        Write(headingText);
                        Write("\n");
                        Write(divider);
                    }

                    Write("\n\n");
                    break;

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

                    string itemText = trav.GetString().Trim();

                    if (!string.IsNullOrWhiteSpace(itemText))
                    {
                        Write("* ");
                        Write(itemText);
                        Write("\n");
                    }

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

                case "TR":
                    // Start of table row
                    Write("| ");

                    TraverseChildren(element);

                    // Separate rows with an empty line
                    Write("\n\n");

                    break;

                case "TD":
                case "TH":
                    TraverseChildren(element);

                    // Separate table columns with a symbol
                    Write(" | ");

                    break;

                case "A":
                    string hrefAttribute = element.GetAttribute("href");
                    var linkIndex = -1;

                    if (!string.IsNullOrWhiteSpace((hrefAttribute)))
                    {
                        if (links.Contains(hrefAttribute))
                        {
                            linkIndex = links.IndexOf(hrefAttribute) + 1;
                        }
                        else
                        {
                            links.Add(hrefAttribute);
                            linkIndex = links.Count();
                        }
                    }

                    TraverseChildren(element);

                    if (linkIndex >= 0)
                    {
                        Write($" ({linkIndex})");
                    }
                    break;

                default:
                    TraverseChildren(element);
                    break;
            }
        }

        private void Write(string text)
        {
            foreach (char c in text)
            {
                if (c == '\n')
                {
                    // Write no more than 1 empty line (\n twice)
                    if (this.newLinesCount < 2)
                    {
                        // Remove space at the end of the previous line
                        if (newLinesCount == 0 && lastWasSpace)
                        {
                            this.output.Length--;
                        }

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
