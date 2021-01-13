using Xunit;

namespace Textify.Tests
{
    public class LinksTests : BaseTest
    {
        [Theory]
        [InlineData("<a href=\"link\">Test</a>", "Test [1]\n\n[1] link")]
        [InlineData("<a href=\"link\">Test</a> suff", "Test [1] suff\n\n[1] link")]
        [InlineData("<a href=\"link\">Test</a>  suff", "Test [1] suff\n\n[1] link")]
        [InlineData("Prefix. <a href=\"link\">Test</a>", "Prefix. Test [1]\n\n[1] link")]
        [InlineData("Prefix. \n<a href=\"link\">Test</a>", "Prefix. Test [1]\n\n[1] link")]
        [InlineData("Prefix.\n<a href=\"link\">Test</a>", "Prefix. Test [1]\n\n[1] link")]
        [InlineData("Prefix.\n <a href=\"link\">Test</a>", "Prefix. Test [1]\n\n[1] link")]
        [InlineData("<a href=\"link\">Test</a> and <a href=\"link2\">Another</a> and <a href=\"link\">Test Again</a>", "Test [1] and Another [2] and Test Again [1]\n\n[1] link\n[2] link2")]
        public void ShouldConvertLinks(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("Hi <a href=\"link\"></a>---", "Hi [1]---\n\n[1] link")]
        [InlineData("Hi<a href=\"link\"></a>.", "Hi [1].\n\n[1] link")]
        public void ShouldCollapseEmptyLinks(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
