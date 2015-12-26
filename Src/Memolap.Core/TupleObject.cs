namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject<T>
    {
        private IList<Dimension> dimensions;
        private short[] values;

        public TupleObject(IList<Dimension> dimensions)
        {
            this.dimensions = dimensions;
            this.values = new short[dimensions.Count];
            this.Data = default(T);
        }

        public TupleObject(IList<Dimension> dimensions, short[] values, T data)
        {
            this.dimensions = dimensions;
            this.values = values;
            this.Data = data;
        }

        public TupleObject(IDictionary<string, object> values)
        {
            this.values = new short[dimensions.Count];

            foreach (var val in values)
            {
                Dimension dimension = dimensions.First(d => d.Name == val.Key);
                int position = dimensions.IndexOf(dimension);
                this.values[position] = dimension.GetValue(val.Value);
            }

            this.Data = default(T);
        }

        public TupleObject(TupleObject<T> tuple)
        {
            this.Data = tuple.Data;
            this.values = new short[tuple.values.Length];
            Array.Copy(tuple.values, this.values, this.values.Length);
            this.dimensions = tuple.dimensions;
        }

        public T Data { get; set; }

        public int Size { get { return this.values.Length; } }

        public bool HasValue(string dimname, object value)
        {
            Dimension dimension = this.dimensions.FirstOrDefault(d => d.Name == dimname);

            if (dimension == null)
                return false;

            int position = this.dimensions.IndexOf(dimension);

            object val = dimension.GetValue(this.values[position]);

            if (value == null)
                return val == null;

            return value.Equals(val);
        }

        public void SetValue(string dimname, object value)
        {
            Dimension dimension = this.dimensions.First(d => d.Name == dimname);
            int position = this.dimensions.IndexOf(dimension);
            this.values[position] = dimension.GetValue(value);
        }

        public object GetValue(string dimname)
        {
            Dimension dimension = this.dimensions.First(d => d.Name == dimname);
            int position = this.dimensions.IndexOf(dimension);
            var value = this.values[position];

            return dimension.GetValue(value);
        }

        public IEnumerable<object> GetValues()
        {
            object[] vals = new object[this.dimensions.Count];

            for (int k = 0; k < vals.Length; k++)
                vals[k] = this.dimensions[k].GetValue(this.values[k]);

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
