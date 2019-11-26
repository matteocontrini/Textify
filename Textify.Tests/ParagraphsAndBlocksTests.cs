using Xunit;

namespace Textify.Tests
{
    public class ParagraphsAndBlocksTests : BaseTest
    {
        [Theory]
        [InlineData("Test<br>", "Test")]
        [InlineData("Test<br/>", "Test")]
        [InlineData("Test<br />", "Test")]
        [InlineData("Test<BR>", "Test")]
        [InlineData("Test <br>", "Test")]
        [InlineData("Test <br> Hey", "Test\nHey")]
        [InlineData("Test <br>Hey", "Test\nHey")]
        [InlineData("Test<br>Hey", "Test\nHey")]
        public void ShouldConvertNewLines(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<p>Test</p>", "Test")]
        [InlineData("<p>Test</p> hello", "Test\n\nhello")]
        [InlineData("<p>Test</p><p>Test 2</p>", "Test\n\nTest 2")]
        [InlineData("\n<p>Test</p>\n\n\t<p>Test 2</p>", "Test\n\nTest 2")]
        [InlineData("<p>Test</p><br><p>Test 2</p>\n", "Test\n\nTest 2")]
        [InlineData("<p>Test</p><br>\n \t \n<p>Test 2</p>\n", "Test\n\nTest 2")]
        public void ShouldConvertParagraphs(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<div>Test</div>", "Test")]
        [InlineData("<div>Test</div> hello", "Test\nhello")]
        [InlineData("<div>Test</div><div>Test 2</div>", "Test\nTest 2")]
        [InlineData("<div>Test</div>\n\t\n<div>Test 2</div>", "Test\nTest 2")]
        [InlineData("<div>Test</div>\n\t\n<p>Test 2</p>", "Test\n\nTest 2")]
        [InlineData("<p>Test</p>\n\t\n<div>Test 2</div>", "Test\n\nTest 2")]
        public void ShouldConvertDivs(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<div><p>Test</p></div>", "Test")]
        [InlineData("<div>Pre <p>Test</p></div>", "Pre\n\nTest")]
        [InlineData("<div>Pre \n\t<p>Test</p></div>", "Pre\n\nTest")]
        [InlineData("<div>Pre <p>Test <a>link</a></p></div>", "Pre\n\nTest link")]
        [InlineData("<div>Pre <p>Test</p><div>Ha</div></div>", "Pre\n\nTest\n\nHa")]
        [InlineData("<div>   Pre   <p>       Test</p>\n\n    \n<div>   Ha </div> </div>", "Pre\n\nTest\n\nHa")]
        public void ShouldConvertNestedBlocks(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
