namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;

    public interface IQuery<T>
    {
        IEnumerable<TupleObject<T>> Tuples { get; }

        IQuery<T> Where(IDictionary<string, object> values);
    }
}
