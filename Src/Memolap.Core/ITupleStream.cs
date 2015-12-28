namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;

    public interface ITupleStream<T>
    {
        IList<Dimension> Dimensions { get; }

        IEnumerable<TupleObject<T>> Tuples { get; }
    }
}
