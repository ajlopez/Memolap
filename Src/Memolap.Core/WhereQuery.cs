namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WhereQuery<T> : BaseQuery<T>, IQuery<T>
    {
        private IQuery<T> query;
        private IDictionary<string, object> values;

        public WhereQuery(IQuery<T> query, IDictionary<string, object> values)
        {
            this.query = query;
            this.values = values;
        }

        public override IEnumerable<TupleObject<T>> Tuples
        {
            get
            {
                foreach (var tuple in this.query.Tuples)
                    if (tuple.Match(this.values))
                        yield return tuple;
            }
        }
    }
}
