namespace GyoumuLib.QueryObjects
{
    internal static class QueryCommon
    {
        private const string INVALID_CHARS = ". +-*/,\"'";

#if NET8_0_OR_GREATER
        private static readonly System.Buffers.SearchValues<char> _invalidChars = System.Buffers.SearchValues.Create(INVALID_CHARS);
#else
        private static readonly char[] _invalidChars = INVALID_CHARS.ToArray();
#endif

        public static void ValidateColumnName(string columnName, string paramName = "columnName")
        {
            ANE.ThrowIfNullOrEmpty(columnName, paramName);

#if NET8_0_OR_GREATER
            var s = columnName.AsSpan();
#else
            var s = columnName;
#endif
            var found = s.IndexOfAny(_invalidChars);
            if (found >= 0)
                throw new ArgumentException($"Invalid column name. Invalid char {s[found]}", paramName);
        }
    }
}
