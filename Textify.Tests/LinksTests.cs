using Xunit;

namespace Textify.Tests
{
    public class LinksTests : BaseTest
    {
        [Theory]
        [InlineData("<a href=\"link\">Test</a>", "Test")]
        [InlineData("<a href=\"link\">Test</a> suff", "Test suff")]
        [InlineData("<a href=\"link\">Test</a>  suff", "Test suff")]
        [InlineData("Prefix. <a href=\"link\">Test</a>", "Prefix. Test")]
        [InlineData("Prefix. \n<a href=\"link\">Test</a>", "Prefix. Test")]
        [InlineData("Prefix.\n<a href=\"link\">Test</a>", "Prefix. Test")]
        [InlineData("Prefix.\n <a href=\"link\">Test</a>", "Prefix. Test")]
        public void ShouldConvertLinks(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("Hi <a href=\"link\"></a>---", "Hi ---")]
        [InlineData("Hi<a href=\"link\"></a>.", "Hi.")]
        public void ShouldCollapseEmptyLinks(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
