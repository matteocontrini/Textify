using Xunit;

namespace Textify.Tests
{
    public class TablesTests : BaseTest
    {
        [Theory]
        // single line
        [InlineData("<table><tr><td>Test 1</td><td>Test 2</td></tr></table>", "Test 1 | Test 2 |")]
        // multiple lines
        [InlineData("<table><tr><td>Test 1</td><td>Test 2</td></tr><tr><td>Test 3</td></tr></table>", "Test 1 | Test 2 |\n\nTest 3 |")]
        // with spaces
        [InlineData("<table>\n\n<tr><td>\nTest  1\t</td>\n\t<td>Test 2</td></tr>\n<tr><td>Test 3</td>\n</tr></table>", "Test 1 | Test 2 |\n\nTest 3 |")]
        // with new line
        [InlineData("<table><tr><td>Test\nLine</td></tr></table>", "Test Line |")]
        [InlineData("<table><tr><td>Test<br>Line</td></tr></table>", "Test\nLine |")]
        public void ShouldConvertTables(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<table><tr><th>Heading</th></tr><tr><td>Body</td></tr></table>", "Heading |\n\nBody |")]
        public void ShouldConvertTablesWithHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
