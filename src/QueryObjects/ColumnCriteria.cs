using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal abstract class ColumnCriteria : Criteria
    {
        [DataMember]
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
