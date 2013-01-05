namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tuple
    {
        private IList<Value> values;

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

        public int Size { get { return this.values.Count; } }

        public bool HasValue(string dimension, object value)
        {
            return this.values.Any(v => v.Dimension.Name.Equals(dimension) && v.Object.Equals(value));
        }
    }
}
