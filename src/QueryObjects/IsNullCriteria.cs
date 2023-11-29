using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class IsNullCriteria : ColumnCriteria
    {
        private readonly bool IsNotNull;

        public IsNullCriteria(string columnName, bool isNotNull)
            : base(columnName)
        {
            IsNotNull = isNotNull;
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
        {
            if (IsNotNull)
                AppendCondition(query, builder, $"{Column} IS NOT NULL");
            else
                AppendCondition(query, builder, $"{Column} IS NULL");
        }
    }
}
