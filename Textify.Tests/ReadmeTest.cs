using Xunit;

namespace Textify.Tests
{
    public class ReadmeTest : BaseTest
    {
        [Fact]
        public void ReadmeShouldRenderAsDisplayed()
        {
            string input = @"<div id=""page"">
    <header>
        <a href=""http://example.com"" class=""site-logo"">
        	<img src=""logo.png"" alt=""Logo"" />
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
</div>";

            string expected = @"[IMG: Logo] [1]

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

[1] http://example.com".Replace("\r\n","\n");

            RunConversion(input, expected);
        }

    }
}
