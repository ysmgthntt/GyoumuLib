using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class BetweenCriteria : ColumnCriteria
    {
        private readonly object StartValue;
        private readonly object EndValue;

        public BetweenCriteria(string columnName, object startValue, object endValue)
            : base(columnName)
        {
            ANE.ThrowIfNull(startValue);
            ANE.ThrowIfNull(endValue);

            StartValue = startValue;
            EndValue = endValue;
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
            => AppendCondition(query, builder, $"{Column} BETWEEN {StartValue} AND {EndValue}");
    }
}
