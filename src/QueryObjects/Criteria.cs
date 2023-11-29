using System.Text;

namespace GyoumuLib.QueryObjects
{
    public abstract class Criteria
    {
        protected internal abstract void AppendCondition(StringBuilder query, QueryBuilder builder);
    }
}
