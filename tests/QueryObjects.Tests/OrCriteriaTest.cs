#nullable disable

using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class OrCriteriaTest
    {
        [Fact]
        public void EmptyTest()
        {
            var target = new OrCriteria();
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Empty(where);
        }

        [Fact]
        public void AddTest()
        {
            var target = new OrCriteria();
            target.Eq("colA", "valA");
            target.Add(new OrCriteria().Eq("col1", 1).Or.Eq("col2", "val2"));
            target.Eq("colB", "valB");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') OR (([col1] = 1) OR ([col2] = 'val2')) OR ([colB] = 'valB')", where);
        }

        [Fact]
        public void AddNullTest()
        {
            var target = new OrCriteria();
            Assert.Throws<ArgumentNullException>("criteria", () => target.Add(null));
        }

        [Fact]
        public void EqTest()
        {
            var target = new OrCriteria();
            target.Eq("colA", "valA").Or.Eq("colB", "valB");
            target.Eq("colC", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') OR ([colB] = 'valB') OR ([colC] = 'valC')", where);
        }

        [Fact]
        public void CpTest()
        {
            var target = new OrCriteria();
            target.Cp("colA", "<>", "valA").Or.Cp("colB", ">", "valB");
            target.Cp("colC", "<", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] <> 'valA') OR ([colB] > 'valB') OR ([colC] < 'valC')", where);
        }

        [Fact]
        public void LikeTest()
        {
            var target = new OrCriteria();
            target.Like("colA", "valA").Or.Like("colB", "valB", true, false);
            target.Like("colC", "valC", false, true);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] LIKE '%valA%') OR ([colB] LIKE 'valB%') OR ([colC] LIKE '%valC')", where);
        }

        [Fact]
        public void BetweenTest()
        {
            var target = new OrCriteria();
            target.Between("colA", 1, 2).Or.Between("colB", 3, 4);
            target.Between("colC", "A", "Z");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] BETWEEN 1 AND 2) OR ([colB] BETWEEN 3 AND 4) OR ([colC] BETWEEN 'A' AND 'Z')", where);
        }

        [Fact]
        public void IsNullTest()
        {
            var target = new OrCriteria();
            target.IsNull("colA").Or.IsNotNull("colB");
            target.IsNull("colC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] IS NULL) OR ([colB] IS NOT NULL) OR ([colC] IS NULL)", where);
        }

        [Fact]
        public void InTest()
        {
            var target = new OrCriteria();
            target.In("colA", 1, 2).Or.In("colB", 3, 4, 5);
            target.In("colC", "A", "B", "C");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] IN (1, 2)) OR ([colB] IN (3, 4, 5)) OR ([colC] IN ('A', 'B', 'C'))", where);
        }

        [Fact]
        public void NotTest()
        {
            var target = new OrCriteria();
            target.Eq("colA", "valA").Or.Not.Eq("colB", "valB").Or.Eq("colC", "valC");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') OR (NOT ([colB] = 'valB')) OR ([colC] = 'valC')", where);
        }

        [Fact]
        public void NotNotTest()
        {
            var target = new OrCriteria();
            target.Eq("colA", "valA").Or.Not.Not.Eq("colB", "valB");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') OR (NOT (NOT ([colB] = 'valB')))", where);
        }

        [Fact]
        public void NotAddTest()
        {
            var target = new OrCriteria();
            target.Eq("colA", "valA").Or.Not.Add(new OrCriteria().Eq("colB", "valB").Or.Eq("colC", "valC")).Or.Eq("colD", "valD");
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 'valA') OR (NOT (([colB] = 'valB') OR ([colC] = 'valC'))) OR ([colD] = 'valD')", where);
        }

        [Fact]
        public void DCSTest()
        {
            Criteria Create() => new OrCriteria().Eq("colA", 123).Or.Eq("colB", 456);
            var target = DCS.SerializeAndDeserialize(Create());

            Assert.IsType<CompoundCriteria>(target);
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) OR ([colB] = 456)", where);
        }

        [Fact]
        public void MessagePackTest()
        {
            Criteria Create() => new OrCriteria().Eq("colA", 123).Or.Eq("colB", 456);
            var bin = MessagePackSerializer.Serialize(Create(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<Criteria>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);

            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target);
            Assert.Equal("([colA] = 123) OR ([colB] = 456)", where);
        }
    }
}
