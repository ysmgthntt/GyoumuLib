using System.Runtime.CompilerServices;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal abstract class ColumnCriteria : Criteria
    {
        internal readonly string ColumnName;

        protected ColumnCriteria(string columnName)
        {
            QueryCommon.ValidateColumnName(columnName);

            ColumnName = columnName;
        }

        protected ColumnCriteria Column => this;

        private protected void AppendCondition(StringBuilder query, QueryBuilder builder
            , [InterpolatedStringHandlerArgument("", nameof(query), nameof(builder))] ref QueryBuilder.AppendConditionHandler _)
        { }
    }
}
