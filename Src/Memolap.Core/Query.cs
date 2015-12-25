namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Query<T> : BaseQuery<T>, IQuery<T>
    {
        private TupleSet<T> tupleset;

        public Query(TupleSet<T> tupleset)
        {
            this.tupleset = tupleset;
        }

        public override IEnumerable<TupleObject<T>> GetTuples()
        {
            return this.tupleset.GetTuples();
        }
    }
}
