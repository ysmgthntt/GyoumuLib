using System.Buffers;

namespace GyoumuLib.QueryObjects
{
    internal static class QueryCommon
    {
        private static readonly SearchValues<char> _invalidChars = SearchValues.Create(". +-*/,");

        public static void ValidateColumnName(string columnName, string paramName = "columnName")
        {
            ANE.ThrowIfNullOrEmpty(columnName);

            var s = columnName.AsSpan();
            var found = s.IndexOfAny(_invalidChars);
            if (found >= 0)
                throw new ArgumentException($"Invalid column name. Invalid char {s[found]}", paramName);
        }
    }
}
