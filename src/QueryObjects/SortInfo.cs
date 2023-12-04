using System.Runtime.Serialization;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    internal sealed class SortInfo
    {
        [DataMember]
        internal readonly string ColumnName;

        [DataMember]
        internal readonly bool IsDescending;

        internal SortInfo(string columnName, bool isDescending)
        {
            QueryCommon.ValidateColumnName(columnName);

            ColumnName = columnName;
            IsDescending = isDescending;
        }
    }
}
