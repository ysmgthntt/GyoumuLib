using System.Runtime.CompilerServices;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    partial class QueryBuilder
    {
        [InterpolatedStringHandler]
        internal readonly ref struct AppendConditionHandler
        {
            private readonly string _columnName;
            private readonly StringBuilder _query;
            private readonly QueryBuilder _builder;

            public AppendConditionHandler(int literalLength, int formattedCount, ColumnCriteria columnCriteria, StringBuilder query, QueryBuilder builder)
            {
                _columnName = columnCriteria.ColumnName;
                _query = query;
                _builder = builder;

                QueryCommon.ValidateColumnName(_columnName, nameof(columnCriteria));
            }

            public void AppendLiteral(string value)
                => _query.Append(value);

            public void AppendLiteral(char value)
                => _query.Append(value);

            public void AppendFormatted(string value)
                => _query.Append(value);

            public void AppendFormatted(ColumnCriteria _)
                => _builder.AppendUniqueColumnName(_query, _columnName);

            public void AppendFormatted(object value)
            {
                var paramName = _builder.AddParameter(_columnName, value);
                _query.Append(paramName);
            }

            public void AppendFormatted(object[] values)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (i > 0)
                        _query.Append(", ");
                    var paramName = _builder.AddParameter(_columnName, values[i]);
                    _query.Append(paramName);
                }
            }
        }
    }
}
