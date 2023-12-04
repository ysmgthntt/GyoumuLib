#nullable disable

using MessagePack.Resolvers;

namespace QueryObjects.Tests
{
    public class SortInfoTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new SortInfo(null, true));
            Assert.Throws<ArgumentException>("columnName", () => new SortInfo("", true));
            Assert.Throws<ArgumentException>("columnName", () => new SortInfo(" ", true));
        }

        [Theory]
        [InlineData("colA", true)]
        [InlineData("colB", false)]
        public void SortTest(string columnName, bool isDesc)
        {
            var target = new SortInfo(columnName, isDesc);
            Assert.Equal(columnName, target.ColumnName);
            Assert.Equal(isDesc, target.IsDescending);
        }

        [Fact]
        public void DCSTest()
        {
            SortInfo Create() => new SortInfo("col", true);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.Equal("col", target.ColumnName);
            Assert.True(target.IsDescending);
        }

        [Fact]
        public void MessagePackTest()
        {
            SortInfo Create() => new SortInfo("col", true);
            var bin = MessagePackSerializer.Serialize(Create(), StandardResolverAllowPrivate.Options);
            var target = MessagePackSerializer.Deserialize<SortInfo>(bin, StandardResolverAllowPrivate.Options);

            Assert.Equal("col", target.ColumnName);
            Assert.True(target.IsDescending);
        }
    }
}
