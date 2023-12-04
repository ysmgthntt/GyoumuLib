using System.Runtime.Serialization;

namespace GyoumuLib.QueryObjects
{
    [DataContract]  // for MessagePack
    public sealed class AndCriteria : CompoundCriteriaBuilder<AndCriteria>
    {
        public AndCriteria() : base(CompoundOperator.And) { }

        // for MessagePack
        private AndCriteria(CompoundCriteria criteria)
            : base(criteria)
        {
            if (criteria.Op != CompoundOperator.And)
                throw new ArgumentException("criteria.Op must be 'AND'", nameof(criteria));
        }

        protected override AndCriteria Next => this;

        public AndCriteria And(Criteria criteria)
        {
            ANE.ThrowIfNull(criteria);
            AddCriteria(criteria);
            return this;
        }
    }
}
