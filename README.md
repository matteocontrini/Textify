# Textify

[![build](https://github.com/matteocontrini/Textify/workflows/Build%20and%20tests/badge.svg)](https://github.com/matteocontrini/Textify/actions) [![codecov](https://codecov.io/gh/matteocontrini/Textify/branch/master/graph/badge.svg)](https://codecov.io/gh/matteocontrini/Textify) [![NuGet](https://img.shields.io/nuget/v/Textify?color=success)](https://www.nuget.org/packages/Textify) [![License](https://img.shields.io/github/license/matteocontrini/Textify?color=success)](https://github.com/matteocontrini/Textify/blob/master/LICENSE)

An HTML to plaintext conversion library for **.NET Standard 2.0** written in C#.

## Features

- Supports HTML **headings, paragraphs, containers, lists and tables** (basic support)
- Takes an **HTML string** as an input or an `INode` from [AngleSharp](https://github.com/AngleSharp/AngleSharp)
- Outputs a readable text representation of the web page
- Targets **.NET Standard 2.0**
- **Full test coverage**

## Installation

Install [from NuGet](https://www.nuget.org/packages/Textift/):

```powershell
Install-Package Textify
```

or

```powershell
dotnet add package Textify
```

## Usage

```csharp
HtmlToTextConverter converter = new HtmlToTextConverter();
string output = converter.Convert(html);
```

By default, the whole page will be converted.

If you're interested in converting only a part of it, parse the page by yourself with AngleSharp and pass the `INode` you're interested in. You don't need to install AngleSharp because Textify already depends on it.

```csharp
HtmlParser parser = new HtmlParser();
IHtmlDocument doc = parser.ParseDocument(html);
IElement element = doc.QuerySelector("#main");

HtmlToTextConverter converter = new HtmlToTextConverter();
string output = converter.Convert(element);
```

## Example

**Input:**

```html
<div id="page">
    <header>
        <a href="http://example.com" class="site-logo">
        	<img src="logo.png" alt="Logo" />
        </a>
        <h1>
            Site title
        </h1>
    </header>
    <main>
    	<article>
        	<h2>Article title</h2>
            
            <p>
                <strong>Lorem ipsum</strong> dolor sit amet, consectetur adipiscing elit,
                sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
            </p>

            <p>Ut enim ad minim veniam, quis nostrud exercitation ullamco
            laboris nisi ut aliquip ex ea commodo consequat.</p>
            
            Here is a list of things anyway:

            <ul>
                <li>One</li>
                <li>Two</li>
                <li>Three</li>
            </ul>

            But maybe a table is nicer:<br><br>
            
            <table>
                <thead>
                	<th>Key</th>
                    <th>Value</th>
                </thead>
                <tr>
                	<td>One</td>
                    <td>Value</td>
                </tr>
            </table>
        </article>
    </main>
</div>
```

Output:

```
[IMG: Logo] [1]

++++++++++
Site title
++++++++++

-------------
Article title
-------------

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.

Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.

Here is a list of things anyway:

* One
* Two
* Three

But maybe a table is nicer:

| Key | Value |

| One | Value |

[1] http://example.com```

## License

MIT license.

Thanks to Jay Taylor for the inspiration with his [html2text](https://github.com/jaytaylor/html2text) Go module.

