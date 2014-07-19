namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject
    {
        private TupleSet tupleset;
        private IList<int> values;

        public TupleObject(TupleSet tupleset)
        {
            this.tupleset = tupleset;
            this.values = new int[tupleset.Dimensions.Count];
            this.Data = null;
        }

        public TupleObject(TupleSet tupleset, IList<int> values, object data)
        {
            this.tupleset = tupleset;
            this.values = values;
            this.Data = data;
        }

        public TupleObject(IDictionary<string, object> values)
        {
            this.values = new int[this.tupleset.Dimensions.Count];

            foreach (var val in values)
            {
                Dimension dimension = this.tupleset.GetDimension(val.Key);
                int position = this.tupleset.GetDimensionOffset(val.Key);
                this.values[position] = dimension.GetValue(val.Value);
            }
        }

        public TupleObject(TupleObject tuple)
        {
            this.Data = tuple.Data;
            this.values = new List<int>(tuple.values);
            this.tupleset = tuple.tupleset;
        }

        public object Data { get; set; }

        public int Size { get { return this.values.Count; } }

        public bool HasValue(string dimension, object value)
        {
            int position = this.tupleset.GetDimensionOffset(dimension);

            if (position < 0)
                return false;

            object val = this.tupleset.GetDimension(dimension).GetValue(this.values[position]);

            if (value == null)
                return val == null;

            return value.Equals(val);
        }

        public void SetValue(string dimension, object value)
        {
            Dimension dim = this.tupleset.GetDimension(dimension);
            int position = this.tupleset.GetDimensionOffset(dimension);
            this.values[position] = dim.GetValue(value);
        }

        public object GetValue(string dimension)
        {
            var value = this.values[this.tupleset.GetDimensionOffset(dimension)];

            if (value == -1)
                return null;

            return this.tupleset.GetDimension(dimension).GetValue(value);
        }

        public IEnumerable<object> GetValues()
        {
            object[] vals = new object[this.tupleset.Dimensions.Count];

            for (int k = 0; k < vals.Length; k++)
                vals[k] = this.tupleset.Dimensions[k].GetValue(this.values[k]);

            return vals;
        }

        public bool Match(IDictionary<string, object> values)
        {
            foreach (var val in values)
            {
                object value = val.Value;
                string dimname = val.Key;

                if (this.tupleset.GetDimension(dimname) == null)
                    return false;

                if (value == null)
                    continue;

                if (!this.HasValue(val.Key, val.Value))
                    return false;
            }

            return true;
        }
    }
}
