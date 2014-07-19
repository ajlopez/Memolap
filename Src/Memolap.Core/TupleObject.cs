namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject
    {
        private TupleSet tupleset;
        private IList<Value> values;

        public TupleObject(TupleSet tupleset)
        {
            this.tupleset = tupleset;
            this.values = new Value[tupleset.Dimensions.Count];
            this.Data = null;
        }

        public TupleObject(TupleSet tupleset, IList<Value> values, object data)
        {
            this.tupleset = tupleset;
            this.values = values;
            this.Data = data;
        }

        public TupleObject(IDictionary<string, object> values)
        {
            this.values = new Value[this.tupleset.Dimensions.Count];

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
            this.values = new List<Value>(tuple.values);
            this.tupleset = tuple.tupleset;
        }

        public object Data { get; set; }

        public int Size { get { return this.values.Count; } }

        public bool HasValue(string dimension, object value)
        {
            return this.values.Any(v => v != null && v.Dimension.Name.Equals(dimension) && (value == null || v.Object.Equals(value)));
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

            if (value == null)
                return null;

            return value.Object;
        }

        public IEnumerable<object> GetValues()
        {
            return this.values.Select(v => v.Object);
        }

        public bool Match(IDictionary<string, object> values)
        {
            foreach (var val in values)
                if (!this.HasValue(val.Key, val.Value))
                    return false;

            return true;
        }
    }
}
