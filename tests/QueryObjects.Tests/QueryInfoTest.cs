#nullable disable

using QueryObjects.Tests.MessagePackHelper;

namespace QueryObjects.Tests
{
    public class QueryInfoTest
    {
        [Fact]
        public void AddSortTest()
        {
            var target = new QueryInfo();
            Assert.Throws<ArgumentNullException>("columnName", () => target.AddSort(null));
            Assert.Throws<ArgumentNullException>("columnName", () => target.AddSort(null, true));
            Assert.Throws<ArgumentException>("columnName", () => target.AddSort(""));
            Assert.Throws<ArgumentException>("columnName", () => target.AddSort("", true));
            Assert.Throws<ArgumentException>("columnName", () => target.AddSort(" "));
            Assert.Throws<ArgumentException>("columnName", () => target.AddSort(" ", true));
            target.AddSort("col", true);
        }

        [Fact]
        public void FilterTest()
        {
            var target = new QueryInfo();
            for (int i = 1; i <= 20; i++)
                target.Filter.Eq("col" + i, i);

            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target.Filter);
            Assert.Equal("([col1] = 1) AND ([col2] = 2) AND ([col3] = 3) AND ([col4] = 4) AND ([col5] = 5) AND ([col6] = 6) AND ([col7] = 7) AND ([col8] = 8) AND ([col9] = 9) AND ([col10] = 10)"
                + " AND ([col11] = 11) AND ([col12] = 12) AND ([col13] = 13) AND ([col14] = 14) AND ([col15] = 15) AND ([col16] = 16) AND ([col17] = 17) AND ([col18] = 18) AND ([col19] = 19) AND ([col20] = 20)"
                , where);
        }

        [Theory]
        [InlineData()]
        [InlineData("col1")]
        [InlineData("col1", "col2", "col3")]
        [InlineData("col_1", "col_2")]
        [InlineData("col１", "col２")]
        [InlineData("colあ", "colい")]
        public void SelectColumnsTest(params string[] selectColumns)
        {
            var target = new QueryInfo();
            target.SelectColumns = selectColumns;
            Assert.Equal(selectColumns, target.SelectColumns);
        }

        [Theory]
        [InlineData(null, "col2")]
        [InlineData("", "col2")]
        [InlineData(" ", "col2")]
        [InlineData("*", "col2")]
        [InlineData("col1", null)]
        [InlineData("col1", "")]
        [InlineData("col1", " ")]
        [InlineData("col1", "*")]
        public void SelectColumnsInvalidColumnNameTest(params string[] selectColumns)
        {
            var target = new QueryInfo();
            Assert.Throws<ArgumentException>("value", () => target.SelectColumns = selectColumns);
        }

        private QueryInfo CreateTestQueryInfo()
        {
            var target = new QueryInfo();
            target.Filter
                .Eq("col1", "val1")
                .Cp("col2", "<>", "val2")
                .And(target.NewAnd()
                    .Like("col3", "val3")
                    .Not.Between("col4", 123, 456)
                )
                .And(target.NewOr()
                    .IsNull("col5")
                    .Or
                    .Not.IsNotNull("col6")
                )
                .Not.In("col7", 1, 2, 3)
                ;
            target.AddSort("col1");
            target.AddSort("col2", true);
            target.SelectColumns = ["col3", "col4"];
            target.StartRecord = 2;
            target.MaxRecords = 3;
            return target;
        }

        private void AssertTestQueryInfo(QueryInfo target)
        {
            var builder = TestQueryBuilder.Instance;
            var where = builder.GetWhereString(target.Filter);
            Assert.Equal("([col1] = 'val1')"
                + " AND ([col2] <> 'val2')"
                + " AND ("
                    + "([col3] LIKE '%val3%')"
                    + " AND (NOT ([col4] BETWEEN 123 AND 456))"
                + ")"
                + " AND ("
                    + "([col5] IS NULL)"
                    + " OR (NOT ([col6] IS NOT NULL))"
                + ")"
                + " AND (NOT ([col7] IN (1, 2, 3)))"
                , where);
            Assert.Equal(["col3", "col4"], target.SelectColumns);
            var orderby = builder.GenerateOrderByClause(target);
            Assert.Equal(" ORDER BY [col1], [col2] DESC", orderby);
            Assert.Equal(2, target.StartRecord);
            Assert.Equal(3, target.MaxRecords);
        }

        [Fact]
        public void DCSTest()
        {
            var target = DCS.SerializeAndDeserialize(CreateTestQueryInfo());
            AssertTestQueryInfo(target);
        }

        [Fact]
        public void MessagePackTest()
        {
            var bin = MessagePackSerializer.Serialize(CreateTestQueryInfo(), KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            var target = MessagePackSerializer.Deserialize<QueryInfo>(bin, KnownTypeResolver.StandardAllowPrivateWithKnownTypeOptions);
            AssertTestQueryInfo(target);
        }
    }
}
