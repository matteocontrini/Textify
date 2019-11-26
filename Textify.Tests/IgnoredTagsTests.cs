using Xunit;

namespace Textify.Tests
{
    public class IgnoredTagsTests : BaseTest
    {
        [Theory]
        [InlineData("<style>body { margin: 0; }</style>", "")]
        [InlineData("<p><style>body { margin: 0; }</style></p>", "")]
        [InlineData("<script src=\"aa\"></script>", "")]
        [InlineData("<p><script src=\"aa\"></script></p>", "")]
        [InlineData("<script>Text inside</script>", "")]
        [InlineData("<script type=\"text/javascript\">Text inside</script>", "")]
        public void ShouldIgnoreStylesScripts(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<html><head><!--aaa--></head><body>Content</body></html>", "Content")]
        [InlineData("<html><head><style>body {}</style></head><body>Content</body></html>", "Content")]
        [InlineData("<html><head><title>Title</title></head><body>Content</body></html>", "Content")]
        public void ShouldIgnoreHead(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
