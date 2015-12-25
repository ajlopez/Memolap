namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;

    public interface IQuery<T>
    {
        IEnumerable<TupleObject<T>> GetTuples();

        IQuery<T> Where(IDictionary<string, object> values);
    }
}
