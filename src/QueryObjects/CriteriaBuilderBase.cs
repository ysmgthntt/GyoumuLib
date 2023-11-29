namespace GyoumuLib.QueryObjects
{
    public abstract class CriteriaBuilderBase<TNext>
    {
        protected abstract TNext Next { get; }

        protected abstract void AddCriteria(Criteria criteria);

        public TNext Eq(string columnName, object value)
        {
            AddCriteria(new ComparisonCriteria(columnName, ComparisonOperator.Eq, value));
            return Next;
        }

        public TNext Cp(string columnName, string op, object value)
        {
            AddCriteria(new ComparisonCriteria(columnName, op, value));
            return Next;
        }

        public TNext Like(string columnName, string value)
        {
            AddCriteria(new ComparisonCriteria(columnName, ComparisonOperator.Like, $"%{value}%"));
            return Next;
        }

        public TNext Like(string columnName, string value, bool prefixSearch, bool suffixSearch)
        {
            if (!prefixSearch)
                value = "%" + value;
            if (!suffixSearch)
                value += "%";

            AddCriteria(new ComparisonCriteria(columnName, ComparisonOperator.Like, value));
            return Next;
        }

        public TNext Between(string columnName, object startValue, object endValue)
        {
            AddCriteria(new BetweenCriteria(columnName, startValue, endValue));
            return Next;
        }

        public TNext IsNull(string columnName)
        {
            AddCriteria(new IsNullCriteria(columnName, false));
            return Next;
        }

        public TNext IsNotNull(string columnName)
        {
            AddCriteria(new IsNullCriteria(columnName, true));
            return Next;
        }

        public TNext In(string columnName, params object[] values)
        {
            AddCriteria(new InCriteria(columnName, values));
            return Next;
        }

        public CriteriaBuilderBase<TNext> Not => new NotOperator(this);

        private sealed class NotOperator : CriteriaBuilderBase<TNext>
        {
            private readonly CriteriaBuilderBase<TNext> _parent;

            public NotOperator(CriteriaBuilderBase<TNext> parent)
                => _parent = parent;

            protected override TNext Next => _parent.Next;

            protected override void AddCriteria(Criteria criteria)
                => _parent.AddCriteria(new NotCriteria(criteria));
        }
    }
}
