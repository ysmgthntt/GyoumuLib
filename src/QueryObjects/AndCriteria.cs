namespace GyoumuLib.QueryObjects
{
    public sealed class AndCriteria : CompoundCriteriaBuilder<AndCriteria>
    {
        public AndCriteria() : base(CompoundOperator.And) { }

        protected override AndCriteria Next => this;

        public AndCriteria And(Criteria criteria)
        {
            ANE.ThrowIfNull(criteria);
            AddCriteria(criteria);
            return this;
        }
    }
}
