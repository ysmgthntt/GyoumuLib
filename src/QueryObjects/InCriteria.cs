using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class InCriteria : ColumnCriteria
    {
        private readonly object[] Values;

        public InCriteria(string columnName, object[] values)
            : base(columnName)
        {
            ANE.ThrowIfNull(values);
            if (values.Length == 0)
                throw new ArgumentException($"{nameof(values)} cannot be empty.", nameof(values));

            Values = values;
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
            => AppendCondition(query, builder, $"{Column} IN ({Values})");
    }
}
