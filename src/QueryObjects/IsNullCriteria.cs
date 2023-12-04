using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class IsNullCriteria : ColumnCriteria
    {
        [DataMember]
        private readonly bool IsNotNull;

        internal IsNullCriteria(string columnName, bool isNotNull)
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
