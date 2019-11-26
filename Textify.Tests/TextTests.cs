using Xunit;

namespace Textify.Tests
{
    public class TextTests : BaseTest
    {
        [Theory]
        [InlineData("test1 test2", "test1 test2")]
        [InlineData("    test1 test2", "test1 test2")]
        [InlineData("    test1    test2 ", "test1 test2")]
        [InlineData("test1\ntest2", "test1 test2")]
        [InlineData("\ntest1 \n\ntest2", "test1 test2")]
        [InlineData("Hello \nThere    \n\tTest", "Hello There Test")]
        [InlineData("\ntest1 \t \t test2", "test1 test2")]
        [InlineData("&nbsp;test1 &nbsp;test2", "test1 test2")]
        public void ShouldStripExtraWhitespace(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("My name is: <strong>Nic</strong>", "My name is: Nic")]
        [InlineData("My name is:<strong>Nic</strong>", "My name is:Nic")]
        [InlineData("Visit \"<a href=\"http://example.com\">Example</strong>\"", "Visit \"Example\"")]
        public void ShouldNotStripWhitespaceBetweenNodes(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("Hello \n<p>Test</p>", "Hello\n\nTest")]
        [InlineData("Hello  \n<p>Test</p>", "Hello\n\nTest")]
        [InlineData("Hello   \n<p>Test</p>", "Hello\n\nTest")]
        [InlineData("Hello  \t\n<p>Test</p>", "Hello\n\nTest")]
        [InlineData("Hello   \t\n<div>Test</div>", "Hello\nTest")]
        public void ShouldStripTrailingWhitespace(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<!-- comm  -->", "")]
        [InlineData("Test<!-- comm -->1", "Test1")]
        [InlineData("Test <!-- comm -->1", "Test 1")]
        [InlineData("Test <!-- comm --> 1", "Test 1")]
        public void ShouldIgnoreComments(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
