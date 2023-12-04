#nullable disable

using MessagePack.Resolvers;
using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class IsNullCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new IsNullCriteria(null, false));
            Assert.Throws<ArgumentException>("columnName", () => new IsNullCriteria("", false));
            Assert.Throws<ArgumentException>("columnName", () => new IsNullCriteria(" ", false));
        }

        [Theory]
        [InlineData("[col] IS NULL", "col", false)]
        [InlineData("[col] IS NOT NULL", "col", true)]
        public void IsNullTest(string expected, string columnName, bool isDesc)
        {
            var target = new IsNullCriteria(columnName, isDesc);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new IsNullCriteria("col", true);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<IsNullCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] IS NOT NULL", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new IsNullCriteria("col", true);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<IsNullCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] IS NOT NULL", where);
        }
    }
}
