using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class CompoundCriteria : Criteria
    {
        [DataMember]
        internal readonly CompoundOperator Op;
        [DataMember]
        private readonly List<Criteria> Conditions;

        internal CompoundCriteria(CompoundOperator op)
        {
            Op = op;
            Conditions = new();
        }

        // for MessagePack
        private CompoundCriteria(CompoundOperator op, List<Criteria> conditions)
        {
            ANE.ThrowIfNull(conditions);

            Op = op;
            Conditions = conditions;
        }

        public void Add(Criteria criteria)
        {
            ANE.ThrowIfNull(criteria);

            Conditions.Add(criteria);
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
        {
            int count = Conditions.Count;
            if (count == 0)
                return;

            string op = Environment.NewLine + Op switch
            {
                CompoundOperator.And => " AND ",
                CompoundOperator.Or => " OR ",
                _ => throw new InvalidOperationException()
            };

            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                    query.Append(op);

                query.Append('(');
                Conditions[i].AppendCondition(query, builder);
                query.Append(')');
            }
        }
    }
}
