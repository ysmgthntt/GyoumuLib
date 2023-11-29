using System.Text;

namespace GyoumuLib.QueryObjects
{
    public abstract partial class QueryBuilder
    {
        protected void BuildCondition(StringBuilder query, Criteria root)
        {
            ANE.ThrowIfNull(query);
            ANE.ThrowIfNull(root);

            root.AppendCondition(query, this);
        }

        public bool GenerateSelectClause(StringBuilder query, QueryInfo qi)
        {
            ANE.ThrowIfNull(query);
            ANE.ThrowIfNull(qi);

            if (qi.SelectColumns is null)
                return false;

            bool appended = false;
            foreach (var columnName in qi.SelectColumns)
            {
                if (appended)
                {
                    query.Append(", ");
                }
                else
                {
                    query.Append("SELECT ");
                    appended = true;
                }
                QueryCommon.ValidateColumnName(columnName);
                AppendUniqueColumnName(query, columnName);
            }

            return appended;
        }

        public bool GenerateOrderByClause(StringBuilder query, QueryInfo qi)
        {
            ANE.ThrowIfNull(query);
            ANE.ThrowIfNull(qi);

            var sortlist = qi.SortList;
            var count = sortlist.Count;
            if (count == 0)
                return false;

            query.Append(" ORDER BY ");

            for (int i = 0; i < count; i++)
            {
                var sort = sortlist[i];
                var columnName = sort.ColumnName;
                QueryCommon.ValidateColumnName(columnName);
                if (i > 0)
                    query.Append(", ");
                AppendUniqueColumnName(query, columnName);
                if (sort.IsDescending)
                    query.Append(" DESC");
            }

            return true;
        }

        protected abstract void AppendUniqueColumnName(StringBuilder query, string columnName);

        protected abstract string AddParameter(string sourceColumn, object value);
    }
}
