using System.Runtime.Serialization;
using System.Text;

namespace GyoumuLib.QueryObjects
{
    [DataContract]
    [KnownType(typeof(ComparisonCriteria))]
    [KnownType(typeof(LikeCriteria))]
    [KnownType(typeof(BetweenCriteria))]
    [KnownType(typeof(IsNullCriteria))]
    [KnownType(typeof(InCriteria))]
    [KnownType(typeof(NotCriteria))]
    [KnownType(typeof(CompoundCriteria))]
    public abstract class Criteria
    {
        protected internal abstract void AppendCondition(StringBuilder query, QueryBuilder builder);
    }
}
