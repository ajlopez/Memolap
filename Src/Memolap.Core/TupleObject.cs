﻿namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject<T>
    {
        private IList<Dimension> dimensions;
        private ushort[] values;
        private short offset;
        private T data;

        public TupleObject(IList<Dimension> dimensions)
        {
            this.dimensions = dimensions;
            this.values = new ushort[dimensions.Count];
            this.offset = 0;
            this.data = default(T);
        }

        public TupleObject(IList<Dimension> dimensions, ushort[] values, T data)
        {
            this.dimensions = dimensions;
            this.values = values;
            this.offset = 0;
            this.data = data;
        }

        public TupleObject(IDictionary<string, object> values, T data)
        {
            this.values = new ushort[this.dimensions.Count];
            this.offset = 0;

            foreach (var val in values)
            {
                Dimension dimension = this.dimensions.First(d => d.Name == val.Key);
                int position = this.dimensions.IndexOf(dimension);
                this.values[position] = dimension.GetValue(val.Value);
            }

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

        public void SetValue(string dimname, object value)
        {
            Dimension dimension = this.dimensions.First(d => d.Name == dimname);
            int position = this.dimensions.IndexOf(dimension);
            this.values[position + this.offset] = dimension.GetValue(value);
        }

        public object GetValue(string dimname)
        {
            Dimension dimension = this.dimensions.First(d => d.Name == dimname);
            int position = this.dimensions.IndexOf(dimension);
            var value = this.values[position + this.offset];

            return dimension.GetValue(value);
        }

        public IEnumerable<object> GetValues()
        {
            object[] vals = new object[this.dimensions.Count];

            for (int k = 0; k < vals.Length; k++)
                vals[k] = this.dimensions[k].GetValue(this.values[k + this.offset]);

            return vals;
        }

        public bool Match(IDictionary<string, object> values)
        {
            foreach (var val in values)
            {
                object value = val.Value;
                string dimname = val.Key;

                Dimension dimension = this.dimensions.FirstOrDefault(d => d.Name == dimname);

                if (dimension == null)
                    return false;

                if (value == null)
                    return true;

                if (!this.HasValue(dimname, value))
                    return false;
            }

            return true;
        }
    }
}
