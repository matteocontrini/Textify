using Xunit;

namespace Textify.Tests
{
    public class ImagesTests : BaseTest
    {
        [Theory]
        [InlineData("<img src=\"aaa\">", "")]
        [InlineData("<img src=\"aaa\" alt=\"\">", "")]
        public void ShouldCollapseImagesWithoutAltText(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<img src=\"aaa\" alt=\"Test\">", "[IMG: Test]")]
        [InlineData("<img alt=\"Test\" />", "[IMG: Test]")]
        [InlineData("<img src=\"aaa\" alt=\"Test\nmultiline\">", "[IMG: Test\nmultiline]")]
        [InlineData("<a><img alt=\"My image\"></a>", "[IMG: My image]")]
        public void ShouldShowImagesWithAltText(string input, string expected)
        {
            RunConversion(input, expected);
        }

        [Theory]
        [InlineData("<img alt=\"Test\"> after text", "[IMG: Test] after text")]
        [InlineData("<img alt=\"Test\">  after text", "[IMG: Test] after text")]
        [InlineData("<img alt=\"Test\">\n\tafter text", "[IMG: Test] after text")]
        [InlineData("<a><img alt=\"My image\"></a> ---", "[IMG: My image] ---")]
        [InlineData("<a><img alt=\"My image\"></a> <a>h</a>", "[IMG: My image] h")]
        [InlineData("<img alt=\"My image\"><p>par</p>", "[IMG: My image]\n\npar")]
        [InlineData("<img alt=\"My image\"> \n\t<p>par</p>", "[IMG: My image]\n\npar")]
        [InlineData("<img alt=\"My image\"> \n \n<div>!!!</p>", "[IMG: My image]\n!!!")]
        public void ShouldAddSpaceAfterImage(string input, string expected)
        {
            RunConversion(input, expected);
        }
    }
}
