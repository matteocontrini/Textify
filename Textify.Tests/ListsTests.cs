using Xunit;

namespace Textify.Tests
{
    public class ListsTests : BaseTest
    {
        [Theory]
        [InlineData("<ul></ul>", "")]
        [InlineData("<ul><li>Test 1</li></ul>", "* Test 1")]
        [InlineData("<ul><li>Test 1</li><li>Test 2</li><li>Test 3</li></ul>", "* Test 1\n* Test 2\n* Test 3")]
        public void ShouldConvertLists(string input, string expected)
        {
            RunConversion(input, expected);
        }

        //[Theory]
        //[InlineData("<ul><li>Test 1<ul><li>Nested</li></ul></li</ul>", "* Test 1\n\n\t* Nested")]
        //[InlineData("<ul><li>Test 1<ul><li>Nested</li></ul></li><li>Test 2</li></ul>", "* Test 1\n\n\t* Nested\n\n* Test 2")]
        //// TODO: add spaces
        //public void ShouldRenderNestedLists(string input, string expected)
        //{
        //    RunConversion(input, expected);
        //}
    }
}
