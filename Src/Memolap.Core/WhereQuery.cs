namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WhereQuery<T> : BaseQuery<T>, IQuery<T>
    {
        private IDictionary<string, object> values;

        public WhereQuery(ITupleStream<T> stream, IDictionary<string, object> values)
            : base(stream)
        {
            this.values = values;
        }

        public override IEnumerable<TupleObject<T>> Tuples
        {
            get
            {
                foreach (var tuple in base.Tuples)
                    if (tuple.Match(this.values))
                        yield return tuple;
            }
        }
    }
}
