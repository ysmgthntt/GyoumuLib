#nullable disable

namespace QueryObjects.Tests
{
    public class QueryCommonTest
    {
        [Fact]
        public void ValidColumnNameNulTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => QueryCommon.ValidateColumnName(null));
        }

        [Theory]
        [InlineData("")]
        [InlineData("tbl.col1")]
        [InlineData("col1 col2")]
        [InlineData("col1+col2")]
        [InlineData("col1-col2")]
        [InlineData("col1*col2")]
        [InlineData("col1/col2")]
        [InlineData("col1,col2")]
        [InlineData("'col1'")]
        [InlineData("\"col1\"")]
        public void ValidateColumnNameTest(string value)
        {
            Assert.Throws<ArgumentException>("columnName", () => QueryCommon.ValidateColumnName(value));
        }

        [Fact]
        public void ValidColumnNameParamNameTest()
        {
            Assert.Throws<ArgumentNullException>("test", () => QueryCommon.ValidateColumnName(null, "test"));
            Assert.Throws<ArgumentException>("test", () => QueryCommon.ValidateColumnName("", "test"));
            Assert.Throws<ArgumentException>("test", () => QueryCommon.ValidateColumnName(".", "test"));
        }
    }
}
