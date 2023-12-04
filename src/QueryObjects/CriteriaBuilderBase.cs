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
            AddCriteria(new LikeCriteria(columnName, $"%{value}%"));
            return Next;
        }

        public TNext Like(string columnName, string value, bool prefixSearch, bool suffixSearch)
        {
            AddCriteria(new LikeCriteria(columnName, value, prefixSearch, suffixSearch));
            return Next;
        }

        public TNext Between<TValue>(string columnName, TValue startValue, TValue endValue)
            where TValue : notnull
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

        public TNext In<TValue>(string columnName, params TValue[] values)
            where TValue : notnull
        {
            AddCriteria(new InCriteria(columnName, values.Cast<object>().ToArray()));
            return Next;
        }

        public TNext Add(Criteria criteria)
        {
            ANE.ThrowIfNull(criteria);
            AddCriteria(criteria);
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
