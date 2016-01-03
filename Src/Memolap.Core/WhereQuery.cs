namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class WhereQuery<T> : BaseQuery<T>, IQuery<T>
    {
        private IDictionary<int, ushort> values = new Dictionary<int, ushort>();

        public WhereQuery(ITupleStream<T> stream, IDictionary<string, object> values)
            : base(stream)
        { 
            foreach (var val in values)
            {
                var dimname = val.Key;
                var value = val.Value;

                var dimension = stream.Dimensions.First(d => d.Name == dimname);
                var dimindex = stream.Dimensions.IndexOf(dimension);
                var intvalue = dimension.GetValue(value);

                this.values[dimindex] = intvalue;
            }
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
