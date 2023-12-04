using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class NotCriteria : Criteria
    {
        [DataMember]
        private readonly Criteria Criteria;

        internal NotCriteria(Criteria criteria)
        {
            ANE.ThrowIfNull(criteria);

            Criteria = criteria;
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
        {
            query.Append("NOT (");
            Criteria.AppendCondition(query, builder);
            query.Append(')');
        }
    }
}
