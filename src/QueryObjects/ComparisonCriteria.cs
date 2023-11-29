using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class ComparisonCriteria : ColumnCriteria
    {
        private readonly ComparisonOperator Operator;
        private readonly object Value;

        public ComparisonCriteria(string columnName, ComparisonOperator op, object value)
            : base(columnName)
        {
            ANE.ThrowIfNull(value);

            Operator = op;
            Value = value;
        }

        public ComparisonCriteria(string columnName, string op, object value)
            : this(columnName, ParseOperator(op), value)
        { }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
            => AppendCondition(query, builder, $"{Column} {GetOperatorString(Operator)} {Value}");

        private const string Eq = "=";
        private const string NotEq = "<>";
        private const string LessThan = "<";
        private const string LessThanEq = "<=";
        private const string GreaterThan = ">";
        private const string GreaterThanEq = ">=";
        private const string Like = "LIKE";

        private static string GetOperatorString(ComparisonOperator op)
            => op switch
            {
                ComparisonOperator.Eq => Eq,
                ComparisonOperator.NotEq => NotEq,
                ComparisonOperator.LessThan => LessThan,
                ComparisonOperator.LessThanEq => LessThanEq,
                ComparisonOperator.GreaterThan => GreaterThan,
                ComparisonOperator.GreaterThanEq => GreaterThanEq,
                ComparisonOperator.Like => Like,
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

            if (Like.Equals(op, StringComparison.OrdinalIgnoreCase))
                return ComparisonOperator.Like;

            var validOp = Enum.GetValues(typeof(ComparisonOperator)).Cast<ComparisonOperator>().Select(GetOperatorString).ToArray();
            throw new ArgumentException($"Comparison operator must be one of [{string.Join(", ", validOp)}].", nameof(op));
        }
    }
}
