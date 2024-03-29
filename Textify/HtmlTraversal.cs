﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using AngleSharp.Dom;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Textify
{
    internal class HtmlTraversal
    {
        private readonly List<string> links;
        private readonly StringBuilder output;
        private bool justClosedDiv;
        private int lineLength;
        private int newLinesCount;
        private int depth;
        private bool lastWasSpace;
        private const int DividerLength = 24;

        public HtmlTraversal(List<string> links)
        {
            this.links = links;
            this.output = new StringBuilder();
        }

        public string GetString(bool includeLinks = false)
        {
            // Write out links
            if (includeLinks && this.links.Any())
            {
                Write("\n\n");

                for (int i = 0; i < this.links.Count; i++)
                {
                    string linkText = this.links[i];
                    int linkNumber = i + 1;

                    Write("[");
                    Write(linkNumber.ToString());
                    Write("] ");
                    Write(linkText);

                    if (i < this.links.Count - 1)
                    {
                        Write("\n");
                    }
                }
            }

            return this.output.ToString();
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
                case "H4":
                case "H5":
                case "H6":
                    Write("\n\n");

                    HtmlTraversal headingTraversal = new HtmlTraversal(this.links);
                    headingTraversal.TraverseChildren(element);

                    string headingText = headingTraversal.GetString().Trim();

                    foreach (string line in headingText.Split('\n'))
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        switch (tagName)
                        {
                            case "H1":
                                Write("+++ ");
                                break;
                            case "H2":
                                Write("++ ");
                                break;
                            default:
                                Write("+ ");
                                break;
                        }

                        Write(line);
                        Write("\n");
                    }

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
                    Write("* ");
                    // TODO: the list item might be empty, in that case we should avoid writing the line
                    TraverseChildren(element);
                    Write("\n");
                    break;

                case "P":
                case "UL":
                    Write("\n\n");
                    this.depth++;
                    TraverseChildren(element);
                    this.depth--;
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
                    HtmlTraversal linkTraversal = new HtmlTraversal(this.links);
                    linkTraversal.TraverseChildren(element);
                    string linkText = linkTraversal.GetString();

                    if (!string.IsNullOrWhiteSpace(linkText))
                    {
                        bool shouldReAddNewLine = linkText.EndsWith("\n");
                        Write(linkText.Trim());

                        string hrefAttribute = element.GetAttribute("href");

                        if (!string.IsNullOrWhiteSpace(hrefAttribute) && !hrefAttribute.StartsWith("#"))
                        {
                            int linkNumber;
                            int existingLinkIndex = this.links.IndexOf(hrefAttribute);
                            if (existingLinkIndex >= 0)
                            {
                                linkNumber = existingLinkIndex + 1;
                            }
                            else
                            {
                                this.links.Add(hrefAttribute);
                                linkNumber = this.links.Count;
                            }

                            Write(" [");
                            Write(linkNumber.ToString());
                            Write("]");
                        }

                        if (shouldReAddNewLine)
                        {
                            Write("\n");
                        }
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
                        if (this.newLinesCount == 0 && this.lastWasSpace)
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
                    bool isWhiteSpace = char.IsWhiteSpace(c);

                    if (this.newLinesCount > 0)
                    {
                        if (isWhiteSpace)
                        {
                            // Skip empty characters at the beginning of lines
                            continue;
                        }

                        // Not whitespace, reset the empty lines count
                        this.newLinesCount = 0;
                    }

                    // Avoid writing 2 consecutive spaces
                    if (this.lastWasSpace && isWhiteSpace)
                    {
                        continue;
                    }

                    if (this.lineLength == 0)
                    {
                        for (int tab = 0; tab < this.depth - 1; tab++)
                        {
                            this.output.Append("\t");
                        }
                    }

                    this.output.Append(c);
                    this.lineLength++;
                    this.lastWasSpace = isWhiteSpace;
                }
            }
        }
    }
}
