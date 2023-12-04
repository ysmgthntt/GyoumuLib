using System.Runtime.Serialization;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    public abstract class CompoundCriteriaBuilder<TNext> : CriteriaBuilderBase<TNext>
    {
        [DataMember]
        private readonly CompoundCriteria Criteria;

        private protected CompoundCriteriaBuilder(CompoundOperator op)
        {
            Criteria = new CompoundCriteria(op);
        }

        // for MessagePack
        private protected CompoundCriteriaBuilder(CompoundCriteria criteria)
        {
            ANE.ThrowIfNull(criteria);
            Criteria = criteria;
        }

        protected override sealed void AddCriteria(Criteria criteria)
        {
            Criteria.Add(criteria);
        }

        public static implicit operator Criteria(CompoundCriteriaBuilder<TNext> builder)
            => builder.Criteria;
    }
}
