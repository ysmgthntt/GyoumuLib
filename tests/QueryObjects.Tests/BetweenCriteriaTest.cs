#nullable disable

using MessagePack.Resolvers;
using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class BetweenCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new BetweenCriteria(null, null, null));
            Assert.Throws<ArgumentException>("columnName", () => new BetweenCriteria("", null, null));
            Assert.Throws<ArgumentException>("columnName", () => new BetweenCriteria(" ", null, null));
            Assert.Throws<ArgumentNullException>("startValue", () => new BetweenCriteria("col", null, null));
            Assert.Throws<ArgumentNullException>("endValue", () => new BetweenCriteria("col", "", null));
        }

        [Theory]
        [InlineData("[col] BETWEEN 123 AND 456", "col", 123, 456)]
        [InlineData("[col] BETWEEN 'abc' AND 'xyz'", "col", "abc", "xyz")]
        public void BetweenTest(string expected, string columnName, object startValue, object endValue)
        {
            var target = new BetweenCriteria(columnName, startValue, endValue);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new BetweenCriteria("col", 123, 456);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<BetweenCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] BETWEEN 123 AND 456", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new BetweenCriteria("col", 123, 456);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<BetweenCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] BETWEEN 123 AND 456", where);
        }
    }
}
