using Xunit;

namespace Textify.Tests
{
    public class BaseTest
    {
        public void RunConversion(string input, string expected)
        {
            HtmlToTextConverter converter = new HtmlToTextConverter();
            string output = converter.Convert(input);

            Assert.Equal(expected, output);
        }
    }
}
