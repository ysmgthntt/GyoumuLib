using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class BetweenCriteria : ColumnCriteria
    {
        [DataMember]
        private readonly object StartValue;
        [DataMember]
        private readonly object EndValue;

        internal BetweenCriteria(string columnName, object startValue, object endValue)
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
