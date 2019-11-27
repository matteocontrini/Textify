using Xunit;

namespace Textify.Tests
{
    public class HeadingsTests : BaseTest
    {
        [Theory]
        [InlineData("<h1>Test</h1>", "++++\nTest\n++++")]
        [InlineData("\n<h1>\nTest</h1>", "++++\nTest\n++++")]
        [InlineData("\t<h1>Test1<br>Test2</h1>", "+++++\nTest1\nTest2\n+++++")]
        [InlineData("<h1>Test</h1><h1>Test2</h1>", "++++\nTest\n++++\n\n+++++\nTest2\n+++++")]
        [InlineData("<h1>Test</h1> <h1>Test2</h1>", "++++\nTest\n++++\n\n+++++\nTest2\n+++++")]
        [InlineData("<h2>Test</h2>", "----\nTest\n----")]
        [InlineData("<h1>Test</h1><h2>Test</h2>", "++++\nTest\n++++\n\n----\nTest\n----")]
        [InlineData("<h3>Test</h3>", "Test\n----")]
        [InlineData("<h3><strong>Test</strong></h3>", "Test\n----")]
        [InlineData("<h3> <strong>Test</strong></h3>", "Test\n----")]
        [InlineData("<h3> <strong> Test</strong></h3>", "Test\n----")]
        [InlineData("<h3>Ha. <strong> Test</strong></h3>", "Ha. Test\n--------")]
        [InlineData("<p>Paragraph</p><h2>Title</h2>", "Paragraph\n\n-----\nTitle\n-----")]
        [InlineData("<p>Paragraph</p><h3>Title</h3>", "Paragraph\n\nTitle\n-----")]
        public void ShouldConvertHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<h1>First line<br>Second line</h1>", "+++++++++++\nFirst line\nSecond line\n+++++++++++")]
        [InlineData("<h1>First line<br>  Second line</h1>", "+++++++++++\nFirst line\nSecond line\n+++++++++++")]
        [InlineData("<h1>First line<br>\n\tSecond line</h1>", "+++++++++++\nFirst line\nSecond line\n+++++++++++")]
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
        [InlineData("<h4>Test</h4>", "Test")]
        [InlineData("<h5>Test</h5>", "Test")]
        [InlineData("<h6>Test</h6>", "Test")]
        [InlineData("<div>Hey</div><h6>Test</h6>", "Hey\n\nTest")]
        [InlineData("<h1>Hey</h1><h6>Test</h6>", "+++\nHey\n+++\n\nTest")]
        [InlineData("<h6>Test</h6> Random text", "Test\n\nRandom text")]
        [InlineData("<h6>Test</h6> <span>Text</span>", "Test\n\nText")]
        public void ShouldConvertSimpleHeadings(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
