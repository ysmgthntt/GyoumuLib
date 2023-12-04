using System.Text;

namespace QueryObjects.Tests
{
    internal sealed class TestQueryBuilder : QueryBuilder
    {
        public static readonly TestQueryBuilder Instance = new TestQueryBuilder();

        private TestQueryBuilder() { }

        public string GetWhereString(Criteria criteria)
        {
            var query = new StringBuilder();
            BuildCondition(query, criteria);
            return query.Replace(Environment.NewLine, "").ToString();
        }

        protected override void AppendUniqueColumnName(StringBuilder query, string columnName)
            => query.Append($"[{columnName}]");

        protected override string AddParameter(string sourceColumn, object value)
            => value switch
            {
                null => "NULL",
                string val => $"'{val}'",
                _ => value.ToString()!
            };

        public string GenerateSelectClause(QueryInfo qi)
        {
            var query = new StringBuilder();
            base.GenerateSelectClause(query, qi);
            return query.ToString();
        }

        public string GenerateOrderByClause(QueryInfo qi)
        {
            var query = new StringBuilder();
            base.GenerateOrderByClause(query, qi);
            return query.ToString();
        }
    }
}
