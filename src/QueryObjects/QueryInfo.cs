using System.Runtime.Serialization;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    public sealed class QueryInfo
    {
        [DataMember]
        public readonly AndCriteria Filter;

        [DataMember]
        internal readonly List<SortInfo> SortList;

        [DataMember]
        public int StartRecord { get; set; }

        [DataMember]
        public int MaxRecords { get; set; }

        private IEnumerable<string>? _selectColumns;

        [DataMember]
        public IEnumerable<string>? SelectColumns
        {
            get => _selectColumns;
            set
            {
                if (value is not null)
                {
                    try
                    {
                        foreach (var columnName in value)
                            QueryCommon.ValidateColumnName(columnName, nameof(value));
                    }
                    catch (ArgumentNullException ex)
                    {
                        throw new ArgumentException(ex.Message, nameof(value), ex);
                    }
                }

                _selectColumns = value;
            }
        }

        public QueryInfo()
        {
            Filter = new();
            SortList = new();
        }

        // for MessagePack
        private QueryInfo(AndCriteria filter, List<SortInfo> sortList, int startRecord, int maxRecords, IEnumerable<string>? selectColumns)
        {
            ANE.ThrowIfNull(filter);
            ANE.ThrowIfNull(sortList);

            Filter = filter;
            SortList = sortList;
            StartRecord = startRecord;
            MaxRecords = maxRecords;
            SelectColumns = selectColumns;
        }

        public void AddSort(string columnName)
            => SortList.Add(new SortInfo(columnName, false));

        public void AddSort(string columnName, bool isDescending)
            => SortList.Add(new SortInfo(columnName, isDescending));

        public AndCriteria NewAnd() => new();

        public OrCriteria NewOr() => new();
    }
}
