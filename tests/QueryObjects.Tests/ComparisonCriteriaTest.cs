#nullable disable

using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class ComparisonCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("columnName", () => new ComparisonCriteria(null, ComparisonOperator.Eq, null));
            Assert.Throws<ArgumentException>("columnName", () => new ComparisonCriteria("", ComparisonOperator.Eq, null));
            Assert.Throws<ArgumentException>("columnName", () => new ComparisonCriteria(" ", ComparisonOperator.Eq, null));
            Assert.Throws<ArgumentNullException>("value", () => new ComparisonCriteria("col", ComparisonOperator.Eq, null));

            Assert.Throws<ArgumentNullException>("columnName", () => new ComparisonCriteria(null, "=", null));
            Assert.Throws<ArgumentException>("columnName", () => new ComparisonCriteria("", "=", null));
            Assert.Throws<ArgumentException>("columnName", () => new ComparisonCriteria(" ", "=", null));
            Assert.Throws<ArgumentNullException>("op", () => new ComparisonCriteria("col", null, null));
            Assert.Throws<ArgumentException>("op", () => new ComparisonCriteria("col", "", null));
            Assert.Throws<ArgumentException>("op", () => new ComparisonCriteria("col", "x", null));
        }

        [Theory]
        [InlineData("[col] = ''", "col", ComparisonOperator.Eq, "")]
        [InlineData("[col] = 123", "col", ComparisonOperator.Eq, 123)]
        [InlineData("[col] <> 123", "col", ComparisonOperator.NotEq, 123)]
        [InlineData("[col] < 123", "col", ComparisonOperator.LessThan, 123)]
        [InlineData("[col] <= 123", "col", ComparisonOperator.LessThanEq, 123)]
        [InlineData("[col] > 123", "col", ComparisonOperator.GreaterThan, 123)]
        [InlineData("[col] >= 123", "col", ComparisonOperator.GreaterThanEq, 123)]
        internal void ComparisonOperatorEnumTest(string expected, string columnName, ComparisonOperator op, object value)
        {
            var target = new ComparisonCriteria(columnName, op, value);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Theory]
        [InlineData("[col] = ''", "col", "=", "")]
        [InlineData("[col] = 123", "col", "=", 123)]
        [InlineData("[col] <> 123", "col", "<>", 123)]
        [InlineData("[col] < 123", "col", "<", 123)]
        [InlineData("[col] <= 123", "col", "<=", 123)]
        [InlineData("[col] > 123", "col", ">", 123)]
        [InlineData("[col] >= 123", "col", ">=", 123)]
        internal void ComparisonOperatorStringTest(string expected, string columnName, string op, object value)
        {
            var target = new ComparisonCriteria(columnName, op, value);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new ComparisonCriteria("col", ComparisonOperator.Eq, 123);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<ComparisonCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] = 123", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new ComparisonCriteria("col", ComparisonOperator.Eq, 123);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<ComparisonCriteria>(target);
            Assert.Equal("col", ((ColumnCriteria)target).ColumnName);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("[col] = 123", where);
        }
    }
}
