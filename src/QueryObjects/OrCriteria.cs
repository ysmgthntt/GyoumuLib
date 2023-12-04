using System.Runtime.Serialization;

namespace GyoumuLib.QueryObjects
{
    // OrCriteria はシリアライズされない。
    public sealed class OrCriteria : CompoundCriteriaBuilder<OrCriteria.OrOperator>
    {
        private readonly OrOperator _orop;

        public OrCriteria() : base(CompoundOperator.Or)
        {
            _orop = new OrOperator(this);
        }

        protected override OrOperator Next => _orop;

        public sealed class OrOperator
        {
            private readonly OrCriteria _parent;

            public OrOperator(OrCriteria parent)
                => _parent = parent;

            public OrCriteria Or => _parent;

            public static implicit operator Criteria(OrOperator or)
                => or._parent;
        }
    }
}
