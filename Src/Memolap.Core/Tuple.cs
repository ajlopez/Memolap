namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tuple
    {
        private IList<Value> values;

        public Tuple()
        {
            this.values = new List<Value>();
        }

        public Tuple(Engine engine, IDictionary<string, object> values)
        {
            this.values = new List<Value>();

            foreach (var val in values)
            {
                Dimension dimension = engine.GetDimension(val.Key);
                if (dimension == null)
                    dimension = engine.CreateDimension(val.Key);
                this.values.Add(dimension.GetValue(val.Value));
            }
        }

        public Tuple(Tuple tuple)
        {
            this.values = new List<Value>(tuple.values);
        }

        public int Size { get { return this.values.Count; } }

        public bool HasValue(string dimension, object value)
        {
            return this.values.Any(v => v.Dimension.Name.Equals(dimension) && (value == null || v.Object.Equals(value)));
        }

        public void SetValue(Engine engine, string dimension, object value)
        {
            Dimension dim = engine.GetDimension(dimension);
            if (dim == null)
                dim = engine.CreateDimension(dimension);
            var val = this.values.FirstOrDefault(v => v.Dimension.Name.Equals(dimension));
            if (val != null)
                this.values.Remove(val);
            this.values.Add(new Value(dim, value));
        }

        public Value GetValue(string dimension)
        {
            return this.values.FirstOrDefault(v => v.Dimension.Name.Equals(dimension));
        }

        public ICollection<Value> GetValues()
        {
            return this.values;
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
