using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class ComparisonCriteria : ColumnCriteria
    {
        [DataMember]
        private readonly ComparisonOperator Op;
        [DataMember]
        private readonly object Value;

        internal ComparisonCriteria(string columnName, ComparisonOperator op, object value)
            : base(columnName)
        {
            ANE.ThrowIfNull(value);

            Op = op;
            Value = value;
        }

        internal ComparisonCriteria(string columnName, string op, object value)
            : this(columnName, ParseOperator(op), value)
        { }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
            => AppendCondition(query, builder, $"{Column} {GetOperatorString(Op)} {Value}");

        private const string Eq = "=";
        private const string NotEq = "<>";
        private const string LessThan = "<";
        private const string LessThanEq = "<=";
        private const string GreaterThan = ">";
        private const string GreaterThanEq = ">=";

        private static string GetOperatorString(ComparisonOperator op)
            => op switch
            {
                ComparisonOperator.Eq => Eq,
                ComparisonOperator.NotEq => NotEq,
                ComparisonOperator.LessThan => LessThan,
                ComparisonOperator.LessThanEq => LessThanEq,
                ComparisonOperator.GreaterThan => GreaterThan,
                ComparisonOperator.GreaterThanEq => GreaterThanEq,
                _ => throw new ArgumentOutOfRangeException(nameof(op), op, null),
            };

        private static ComparisonOperator ParseOperator(string op)
        {
            ANE.ThrowIfNullOrEmpty(op);

            switch (op)
            {
                case Eq:
                    return ComparisonOperator.Eq;
                case NotEq:
                    return ComparisonOperator.NotEq;
                case LessThan:
                    return ComparisonOperator.LessThan;
                case LessThanEq:
                    return ComparisonOperator.LessThanEq;
                case GreaterThan:
                    return ComparisonOperator.GreaterThan;
                case GreaterThanEq:
                    return ComparisonOperator.GreaterThanEq;
            }

            var validOp = Enum.GetValues(typeof(ComparisonOperator)).Cast<ComparisonOperator>().Select(GetOperatorString).ToArray();
            throw new ArgumentException($"Comparison operator must be one of [{string.Join(", ", validOp)}].", nameof(op));
        }
    }
}
