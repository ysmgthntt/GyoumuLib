using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class CompoundCriteria : Criteria
    {
        private readonly CompoundOperator Operator;

        private readonly List<Criteria> Conditions = new();

        public CompoundCriteria(CompoundOperator op)
        {
            Operator = op;
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

            string op = Environment.NewLine + Operator switch
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
