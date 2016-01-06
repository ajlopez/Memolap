namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject<T>
    {
        private IList<Dimension> dimensions;
        private ushort[] values;
        private int offset;
        private T data;

        public TupleObject(IList<Dimension> dimensions)
        {
            this.dimensions = dimensions;
            this.values = new ushort[dimensions.Count];
            this.offset = 0;
            this.data = default(T);
        }

        public TupleObject(IList<Dimension> dimensions, ushort[] values, T data)
            : this(dimensions, values, 0, data)
        {
        }

        public TupleObject(IList<Dimension> dimensions, ushort[] values, int offset, T data)
        {
            this.dimensions = dimensions;
            this.values = values;
            this.offset = offset;
            this.data = data;
        }

        public T Data { get { return this.data; } }

        public int Size { get { return this.values.Length; } }

        public bool HasValue(string dimname, object value)
        {
            Dimension dimension = this.dimensions.FirstOrDefault(d => d.Name == dimname);

            if (dimension == null)
                return false;

            int position = this.dimensions.IndexOf(dimension);

            object val = dimension.GetValue(this.values[this.offset + position]);

            if (value == null)
                return val == null;

            return value.Equals(val);
        }

        public object GetValue(string dimname)
        {
            Dimension dimension = this.dimensions.First(d => d.Name == dimname);
            int position = this.dimensions.IndexOf(dimension);
            var value = this.values[position + this.offset];

            return dimension.GetValue(value);
        }

        public TupleObject<T> Clone()
        {
            return new TupleObject<T>(this.dimensions, this.values, this.offset, this.data);
        }

        public bool Match(IDictionary<int, ushort> values)
        {
            foreach (var val in values)
            {
                int ndim = val.Key;
                ushort value = val.Value;

                if (this.values[ndim] != value)
                    return false;
            }

            return true;
        }
    }
}
