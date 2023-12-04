namespace GyoumuLib.QueryObjects
{
    public sealed class QueryInfo
    {
        public AndCriteria Filter { get; } = new();

        internal readonly List<SortInfo> SortList = new();

        public int StartRecord { get; set; }

        public int MaxRecords { get; set; }

        private IEnumerable<string>? _selectColumns;

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

        public void AddSort(string columnName)
            => SortList.Add(new SortInfo(columnName, false));

        public void AddSort(string columnName, bool isDescending)
            => SortList.Add(new SortInfo(columnName, isDescending));

        public AndCriteria NewAnd() => new();

        public OrCriteria NewOr() => new();
    }
}
