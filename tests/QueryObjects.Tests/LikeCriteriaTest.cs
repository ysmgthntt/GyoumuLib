#nullable disable

using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class LikeCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new LikeCriteria(null, null));
            Assert.Throws<ArgumentException>("columnName", () => new LikeCriteria("", null));
            Assert.Throws<ArgumentException>("columnName", () => new LikeCriteria(" ", null));
            Assert.Throws<ArgumentNullException>("value", () => new LikeCriteria("col", null));

            Assert.Throws<ArgumentNullException>("columnName", () => new LikeCriteria(null, null, false, false));
            Assert.Throws<ArgumentException>("columnName", () => new LikeCriteria("", null, false, false));
            Assert.Throws<ArgumentException>("columnName", () => new LikeCriteria(" ", null, false, false));
            Assert.Throws<ArgumentNullException>("value", () => new LikeCriteria("col", null, false, false));
        }

        [Theory]
        [InlineData("[col] LIKE ''", "col", "")]
        [InlineData("[col] LIKE 'val'", "col", "val")]
        public void LikeTest(string expected, string columnName, string value)
        {
            var target = new LikeCriteria(columnName, value);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Theory]
        [InlineData("[col] LIKE '%val%'", "col", "val", false, false)]
        [InlineData("[col] LIKE 'val%'", "col", "val", true, false)]
        [InlineData("[col] LIKE '%val'", "col", "val", false, true)]
        [InlineData("[col] LIKE 'val'", "col", "val", true, true)]
        [InlineData("[col] LIKE ''", "col", "", true, true)]
        public void LikeTest2(string expected, string columnName, string value, bool prefixSearch, bool suffixSearch)
        {
            var target = new LikeCriteria(columnName, value, prefixSearch, suffixSearch);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new LikeCriteria("col", "val", true, false);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<LikeCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] LIKE 'val%'", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new LikeCriteria("col", "val", true, false);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<LikeCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] LIKE 'val%'", where);
        }
    }
}
