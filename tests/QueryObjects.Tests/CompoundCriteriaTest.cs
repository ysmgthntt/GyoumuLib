#nullable disable

using GyoumuLib.QueryObjects;
using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class CompoundCriteriaTest
    {
        [Fact]
        public void AddNullTest()
        {
            var target = new CompoundCriteria(CompoundOperator.And);
            Assert.Throws<ArgumentNullException>("criteria", () => target.Add(null));
        }

        [Theory]
        [InlineData("([col1] = 'val1') AND ([col2] = 'val2') AND ([col3] = 'val3')", CompoundOperator.And)]
        [InlineData("([col1] = 'val1') OR ([col2] = 'val2') OR ([col3] = 'val3')", CompoundOperator.Or)]
        internal void AddTest(string expected, CompoundOperator op)
        {
            var target = new CompoundCriteria(op);
            target.Add(new ComparisonCriteria("col1", ComparisonOperator.Eq, "val1"));
            target.Add(new ComparisonCriteria("col2", ComparisonOperator.Eq, "val2"));
            target.Add(new ComparisonCriteria("col3", ComparisonOperator.Eq, "val3"));
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal(expected, where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create()
            {
                var criteria = new CompoundCriteria(CompoundOperator.Or);
                criteria.Add(new ComparisonCriteria("col1", ComparisonOperator.Eq, "val1"));
                criteria.Add(new ComparisonCriteria("col2", ComparisonOperator.Eq, "val2"));
                criteria.Add(new ComparisonCriteria("col3", ComparisonOperator.Eq, "val3"));
                return criteria;
            }
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<CompoundCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([col1] = 'val1') OR ([col2] = 'val2') OR ([col3] = 'val3')", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create()
            {
                var criteria = new CompoundCriteria(CompoundOperator.Or);
                criteria.Add(new ComparisonCriteria("col1", ComparisonOperator.Eq, "val1"));
                criteria.Add(new ComparisonCriteria("col2", ComparisonOperator.Eq, "val2"));
                criteria.Add(new ComparisonCriteria("col3", ComparisonOperator.Eq, "val3"));
                return criteria;
            }
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([col1] = 'val1') OR ([col2] = 'val2') OR ([col3] = 'val3')", where);
        }
    }
}
