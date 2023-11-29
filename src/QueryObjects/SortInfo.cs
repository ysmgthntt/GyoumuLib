namespace GyoumuLib.QueryObjects
{
    internal sealed class SortInfo
    {
        public readonly string ColumnName;

        public readonly bool IsDescending;

        public SortInfo(string columnName, bool isDescending)
        {
            QueryCommon.ValidateColumnName(columnName);

            ColumnName = columnName;
            IsDescending = isDescending;
        }
    }
}
