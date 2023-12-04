#nullable disable

using MessagePack.Resolvers;
using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class InCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new InCriteria(null, null));
            Assert.Throws<ArgumentException>("columnName", () => new InCriteria("", null));
            Assert.Throws<ArgumentException>("columnName", () => new InCriteria(" ", null));

            Assert.Throws<ArgumentNullException>("values", () => new InCriteria("col", null));
            Assert.Throws<ArgumentException>("values", () => new InCriteria("col", []));
        }

        [Theory]
        [InlineData("[col] IN (123)", "col", 123)]
        [InlineData("[col] IN (123, 'test')", "col", 123, "test")]
        [InlineData("[col] IN (123, 'test', 0)", "col", 123, "test", 0)]
        public void InTest(string expected, string columnName, params object[] values)
        {
            var target = new InCriteria(columnName, values);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new InCriteria("col", [123, "test", 0]);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<InCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] IN (123, 'test', 0)", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new InCriteria("col", [123, "test", 0]);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<InCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] IN (123, 'test', 0)", where);
        }
    }
}
