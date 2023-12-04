#nullable disable

using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class NotCriteriaTest
    {
        [Fact]
        public void ConstructorTest()
        {
            Assert.Throws<ArgumentNullException>("criteria", () => new NotCriteria(null));
        }

        [Fact]
        public void NotTest()
        {
            var target = new NotCriteria(new IsNullCriteria("col", false));
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("NOT ([col] IS NULL)", where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new NotCriteria(new IsNullCriteria("col", false));
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<NotCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("NOT ([col] IS NULL)", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new NotCriteria(new IsNullCriteria("col", false));
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<NotCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("NOT ([col] IS NULL)", where);
        }
    }
}
