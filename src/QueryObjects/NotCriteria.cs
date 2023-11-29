using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class NotCriteria : Criteria
    {
        private readonly Criteria Criteria;

        public NotCriteria(Criteria criteria)
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
