using Xunit;

namespace Textify.Tests
{
    public class ComplexTest : BaseTest
    {
        [Fact]
        public void ComplexShouldRenderAsExpected()
        {
            string input = @"<div id=""page"">
    <header>
        
        <h1>
            <a href=""http://example.com"" class=""site-logo"">
        	<img src=""logo.png"" alt=""Logo"" />
        </a>
        </h1>
    </header>
    <main>
    	<article>

            <h1>Article title</h1>

        	<h2><a name=""anchor1""><img src=""test.gif"" />Article title</a></h2>
            
            <p>
                <strong>Lorem ipsum</strong> dolor sit amet, consectetur adipiscing elit,
                sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
            </p>

            <h2><a name=""anchor2""><svg />Article title</a></h2>

            <p>Ut enim ad minim <a href=""#anchor2"">ignored</a> veniam, <a href=""external"">quis</a> nostrud exercitation ullamco
            laboris nisi ut aliquip ex ea commodo consequat.</p>
            
            Here is a list of things anyway:

            <ul>
                <li>One</li>
                <li><a href=""#anchor2""><b>T</b>wo</a></li>
                <li><a href=""external"">Three</a></li>
                <li><a href=""external""></a></li>
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

            string expected = @"+++ [IMG: Logo] [1]

+++ Article title

++ Article title

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.

++ Article title

Ut enim ad minim ignored veniam, quis [2] nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.

Here is a list of things anyway:

* One
* Two
* Three [2]
*

But maybe a table is nicer:

| Key | Value |

| One | Value |

[1] http://example.com
[2] external".Replace("\r\n","\n");

            RunConversion(input, expected);
        }

    }
}
