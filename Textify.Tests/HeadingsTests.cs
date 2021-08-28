using Xunit;

namespace Textify.Tests
{
    public class HeadingsTests : BaseTest
    {
        [Theory]
        [InlineData("<h1>Test</h1>", "+++ Test")]
        [InlineData("\n<h1>\nTest</h1>", "+++ Test")]
        [InlineData("<h1>Test</h1><h1>Test2</h1>", "+++ Test\n\n+++ Test2")]
        [InlineData("<h1>Test</h1> <h1>Test2</h1>", "+++ Test\n\n+++ Test2")]
        [InlineData("<h2>Test</h2>", "++ Test")]
        [InlineData("<h1>Test</h1><h2>Test</h2>", "+++ Test\n\n++ Test")]
        [InlineData("<h3>Test</h3>", "+ Test")]
        [InlineData("<h3><strong>Test</strong></h3>", "+ Test")]
        [InlineData("<h3> <strong>Test</strong></h3>", "+ Test")]
        [InlineData("<h3> <strong> Test</strong></h3>", "+ Test")]
        [InlineData("<h3>Ha. <strong> Test</strong></h3>", "+ Ha. Test")]
        [InlineData("<p>Paragraph</p><h2>Title</h2>", "Paragraph\n\n++ Title")]
        [InlineData("<p>Paragraph</p><h2>Title</h2>test", "Paragraph\n\n++ Title\n\ntest")]
        [InlineData("<p>Paragraph</p><h3>Title</h3>", "Paragraph\n\n+ Title")]
        [InlineData("<p>Paragraph</p><h3>Title</h3>test", "Paragraph\n\n+ Title\n\ntest")]
        public void ShouldConvertHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<h1>First line<br>Second line</h1>", "+++ First line\n+++ Second line")]
        [InlineData("<h1>First line<br>  Second line</h1>", "+++ First line\n+++ Second line")]
        [InlineData("<h1>First line<br>\n\tSecond line</h1>", "+++ First line\n+++ Second line")]
        public void ShouldConvertMultilineHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<h1></h1>", "")]
        [InlineData("<h1></h1>\nTest", "Test")]
        [InlineData("<h2></h2>", "")]
        [InlineData("<h3></h3>", "")]
        [InlineData("<h4></h4>", "")]
        [InlineData("<h5></h5>", "")]
        [InlineData("<h6></h6>", "")]
        public void ShouldIgnoreEmptyHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<h4>Test</h4>", "+ Test")]
        [InlineData("<h5>Test</h5>", "+ Test")]
        [InlineData("<h6>Test</h6>", "+ Test")]
        [InlineData("<div>Hey</div><h6>Test</h6>", "Hey\n\n+ Test")]
        [InlineData("<h1>Hey</h1><h6>Test</h6>", "+++ Hey\n\n+ Test")]
        [InlineData("<h6>Test</h6> Random text", "+ Test\n\nRandom text")]
        [InlineData("<h6>Test</h6> <span>Text</span>", "+ Test\n\nText")]
        public void ShouldConvertSimpleHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
