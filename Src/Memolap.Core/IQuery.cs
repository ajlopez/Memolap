namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;

    public interface IQuery<T> : ITupleStream<T>
    {
        IQuery<T> Where(IDictionary<string, object> values);

        IQuery<T> Skip(int n);

        IQuery<T> Take(int n);
    }
}
