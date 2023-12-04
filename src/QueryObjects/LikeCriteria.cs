using System.Text;

namespace GyoumuLib.QueryObjects
{
    internal sealed class LikeCriteria : ColumnCriteria
    {
        private readonly string Value;

        internal LikeCriteria(string columnName, string value)
            : base(columnName)
        {
            ANE.ThrowIfNull(value);

            Value = value;
        }

        internal LikeCriteria(string columnName, string value, bool prefixSearch, bool suffixSearch)
            : base(columnName)
        {
            ANE.ThrowIfNull(value);

            if (!prefixSearch)
                value = "%" + value;
            if (!suffixSearch)
                value += "%";

            Value = value;
        }

        protected internal override void AppendCondition(StringBuilder query, QueryBuilder builder)
            => AppendCondition(query, builder, $"{Column} LIKE {(object)Value}");
    }
}
