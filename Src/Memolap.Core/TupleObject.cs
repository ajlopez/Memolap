namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleObject
    {
        private DataBank databank;
        private IList<Value> values;

        public TupleObject(DataBank databank)
        {
            this.databank = databank;
            this.values = new Value[databank.Dimensions.Count];
            this.Data = null;
        }

        public TupleObject(DataBank databank, IList<Value> values, object data)
        {
            this.databank = databank;
            this.values = values;
            this.Data = data;
        }

        public object Data { get; set; }

        public TupleObject(IDictionary<string, object> values)
        {
            this.values = new Value[this.databank.Dimensions.Count];

            foreach (var val in values)
            {
                Dimension dimension = this.databank.GetDimension(val.Key);
                int position = this.databank.GetDimensionOffset(val.Key);
                this.values[position] = dimension.GetValue(val.Value);
            }
        }

        public TupleObject(TupleObject tuple)
        {
            this.Data = tuple.Data;
            this.values = new List<Value>(tuple.values);
            this.databank = tuple.databank;
        }

        public int Size { get { return this.values.Count; } }

        public bool HasValue(string dimension, object value)
        {
            return this.values.Any(v => v != null && v.Dimension.Name.Equals(dimension) && (value == null || v.Object.Equals(value)));
        }

        public void SetValue(string dimension, object value)
        {
            Dimension dim = this.databank.GetDimension(dimension);
            int position = this.databank.GetDimensionOffset(dimension);
            this.values[position] = dim.GetValue(value);
        }

        public object GetValue(string dimension)
        {
            var value = this.values[this.databank.GetDimensionOffset(dimension)];

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
