namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BaseQuery<T> : IQuery<T>
    {
        private IList<Dimension> dimensions;
        private IEnumerable<TupleObject<T>> tuples;

        public BaseQuery(IList<Dimension> dimensions, IEnumerable<TupleObject<T>> tuples) 
        {
            this.dimensions = dimensions;
            this.tuples = tuples;
        }

        public BaseQuery(ITupleStream<T> stream)
            : this(stream.Dimensions, stream.Tuples)
        {
        }

        public IList<Dimension> Dimensions { get { return this.dimensions; } }

        public virtual IEnumerable<TupleObject<T>> Tuples { get { return this.tuples; } }

        public virtual IQuery<T> Where(IDictionary<string, object> values)
        {
            return new WhereQuery<T>(this, values);
        }

        public virtual IQuery<T> Skip(int n)
        {
            this.tuples = this.tuples.Skip(n);
            return this;
        }

        public virtual IQuery<T> Take(int n)
        {
            this.tuples = this.tuples.Take(n);
            return this;
        }
    }
}
