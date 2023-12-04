#nullable disable

using MessagePack.Resolvers;
using Newtonsoft.Json.Converters;
using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class AndCriteriaTest
    {
        [Fact]
        public void EmptyTest()
        {
            var target = new AndCriteria();
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Empty(where);
        }

        [Fact]
        public void AddTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA");
            target.Add(new AndCriteria().Eq("col1", 1).Eq("col2", "val2"));
            target.Eq("colB", "valB");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND (([col1] = 1) AND ([col2] = 'val2')) AND ([colB] = 'valB')", where);
        }

        [Fact]
        public void AndTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA");
            target.And(new AndCriteria().Eq("col1", 1).Eq("col2", "val2"));
            target.Eq("colB", "valB");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND (([col1] = 1) AND ([col2] = 'val2')) AND ([colB] = 'valB')", where);
        }

        [Fact]
        public void AddNullTest()
        {
            var target = new AndCriteria();
            Assert.Throws<ArgumentNullException>("criteria", () => target.Add(null));
            Assert.Throws<ArgumentNullException>("criteria", () => target.And(null));
        }

        [Fact]
        public void EqTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA").Eq("colB", "valB");
            target.Eq("colC", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND ([colB] = 'valB') AND ([colC] = 'valC')", where);
        }

        [Fact]
        public void CpTest()
        {
            var target = new AndCriteria();
            target.Cp("colA", "<>", "valA").Cp("colB", ">", "valB");
            target.Cp("colC", "<", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] <> 'valA') AND ([colB] > 'valB') AND ([colC] < 'valC')", where);
        }

        [Fact]
        public void LikeTest()
        {
            var target = new AndCriteria();
            target.Like("colA", "valA").Like("colB", "valB", true, false);
            target.Like("colC", "valC", false, true);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] LIKE '%valA%') AND ([colB] LIKE 'valB%') AND ([colC] LIKE '%valC')", where);
        }

        [Fact]
        public void BetweenTest()
        {
            var target = new AndCriteria();
            target.Between("colA", 1, 2).Between("colB", 3, 4);
            target.Between("colC", "A", "Z");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] BETWEEN 1 AND 2) AND ([colB] BETWEEN 3 AND 4) AND ([colC] BETWEEN 'A' AND 'Z')", where);
        }

        [Fact]
        public void IsNullTest()
        {
            var target = new AndCriteria();
            target.IsNull("colA").IsNotNull("colB");
            target.IsNull("colC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] IS NULL) AND ([colB] IS NOT NULL) AND ([colC] IS NULL)", where);
        }

        [Fact]
        public void InTest()
        {
            var target = new AndCriteria();
            target.In("colA", 1, 2).In("colB", 3, 4, 5);
            target.In("colC", "A", "B", "C");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] IN (1, 2)) AND ([colB] IN (3, 4, 5)) AND ([colC] IN ('A', 'B', 'C'))", where);
        }

        [Fact]
        public void NotTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA").Not.Eq("colB", "valB").Eq("colC", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND (NOT ([colB] = 'valB')) AND ([colC] = 'valC')", where);
        }

        [Fact]
        public void NotNotTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA").Not.Not.Eq("colB", "valB");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND (NOT (NOT ([colB] = 'valB')))", where);
        }

        [Fact]
        public void NotAddTest()
        {
            var target = new AndCriteria();
            target.Eq("colA", "valA").Not.Add(new AndCriteria().Eq("colB", "valB").Eq("colC", "valC")).Eq("colD", "valD");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') AND (NOT (([colB] = 'valB') AND ([colC] = 'valC'))) AND ([colD] = 'valD')", where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new AndCriteria().Eq("colA", 123).Eq("colB", 456);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<CompoundCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) AND ([colB] = 456)", where);
        }

        [Fact]
        public void DCSAndCriteriaTest()
        {
            AndCriteria Create() => new AndCriteria().Eq("colA", 123).Eq("colB", 456);
            var target = DCS.SerializeAndDeserialize(Create());

            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) AND ([colB] = 456)", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new AndCriteria().Eq("colA", 123).Eq("colB", 456);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            Assert.IsType<CompoundCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) AND ([colB] = 456)", where);
        }

        [Fact]
        public void MessagePackAndCriteriaTest()
        {
            AndCriteria Create() => new AndCriteria().Eq("colA", 123).Eq("colB", 456);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<AndCriteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) AND ([colB] = 456)", where);
        }
    }
}
