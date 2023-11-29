namespace GyoumuLib.QueryObjects
{
    public abstract class CompoundCriteriaBuilder<TNext> : CriteriaBuilderBase<TNext>
    {
        //[]
        private readonly CompoundCriteria Criteria;

        private protected CompoundCriteriaBuilder(CompoundOperator op)
        {
            Criteria = new CompoundCriteria(op);
        }

        protected override sealed void AddCriteria(Criteria criteria)
        {
            Criteria.Add(criteria);
        }

        public static implicit operator Criteria(CompoundCriteriaBuilder<TNext> builder)
            => builder.Criteria;
    }
}
