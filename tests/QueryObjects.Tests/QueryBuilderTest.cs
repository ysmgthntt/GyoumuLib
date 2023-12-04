#nullable disable

namespace QueryObjects.Tests
{
    public class QueryBuilderTest
    {
        [Fact]
        public void GenerateSelectDefaultTest()
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            var select = tartet.GenerateSelectClause(qi);
            Assert.Empty(select);
        }

        [Fact]
        public void GenerateSelectNullTest()
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            qi.SelectColumns = null;
            var select = tartet.GenerateSelectClause(qi);
            Assert.Empty(select);
        }

        [Fact]
        public void GenerateSelectEmptyTest()
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            qi.SelectColumns = [];
            var select = tartet.GenerateSelectClause(qi);
            Assert.Empty(select);
        }

        [Theory]
        [InlineData("SELECT [col1]", "col1")]
        [InlineData("SELECT [col1], [col2]", "col1", "col2")]
        [InlineData("SELECT [col1], [col2], [col3]", "col1", "col2", "col3")]
        public void GenerateSelectTest(string expected, params string[] selectColumns)
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            qi.SelectColumns = selectColumns;
            var select = tartet.GenerateSelectClause(qi);
            Assert.Equal(expected, select);
        }

        [Fact]
        public void GenerateOrderByDefaultTest()
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            var orderby = tartet.GenerateOrderByClause(qi);
            Assert.Empty(orderby);
        }

        public static IEnumerable<object[]> OrderByTestData()
            => [
                [" ORDER BY [col1]", ("col1", (bool?)null)],
                [" ORDER BY [col1]", ("col1", (bool?)false)],
                [" ORDER BY [col1] DESC", ("col1", (bool?)true)],
                [" ORDER BY [col1], [col2]", ("col1", (bool?)null), ("col2", (bool?)null)],
                [" ORDER BY [col1], [col2]", ("col1", (bool?)false), ("col2", (bool?)null)],
                [" ORDER BY [col1], [col2]", ("col1", (bool?)null), ("col2", (bool?)false)],
                [" ORDER BY [col1] DESC, [col2]", ("col1", (bool?)true), ("col2", (bool?)null)],
                [" ORDER BY [col1], [col2] DESC", ("col1", (bool?)null), ("col2", (bool?)true)],
                [" ORDER BY [col1] DESC, [col2] DESC", ("col1", (bool?)true), ("col2", (bool?)true)],
                [" ORDER BY [col1], [col2], [col3]", ("col1", (bool?)null), ("col2", (bool?)null), ("col3", (bool?)null)],
                [" ORDER BY [col1] DESC, [col2], [col3]", ("col1", (bool?)true), ("col2", (bool?)null), ("col3", (bool?)null)],
                [" ORDER BY [col1], [col2] DESC, [col3]", ("col1", (bool?)null), ("col2", (bool?)true), ("col3", (bool?)null)],
                [" ORDER BY [col1], [col2], [col3] DESC", ("col1", (bool?)null), ("col2", (bool?)null), ("col3", (bool?)true)],
                [" ORDER BY [col1] DESC, [col2] DESC, [col3] DESC", ("col1", (bool?)true), ("col2", (bool?)true), ("col3", (bool?)true)],
            ];

        [Theory]
        [MemberData(nameof(OrderByTestData))]
        public void GenerateOrderByTest(string expected, params (string columnName, bool? isDesc)[] order)
        {
            var tartet = TestQueryBuilder.Instance;
            var qi = new QueryInfo();
            foreach (var (columnName, isDesc) in order)
            {
                if (isDesc.HasValue)
                    qi.AddSort(columnName, isDesc.Value);
                else
                    qi.AddSort(columnName);
            }
            var orderby = tartet.GenerateOrderByClause(qi);
            Assert.Equal(expected, orderby);
        }
    }
}
