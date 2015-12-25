namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class BaseQuery<T> : IQuery<T>
    {
        public abstract IEnumerable<TupleObject<T>> GetTuples();

        public IQuery<T> Where(IDictionary<string, object> values)
        {
            return new WhereQuery<T>(this, values);
        }
    }
}
