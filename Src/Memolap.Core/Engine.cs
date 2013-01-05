namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private IDictionary<String, Dimension> dimensions = new Dictionary<String, Dimension>();

        public Dimension CreateDimension(string name)
        {
            if (dimensions.ContainsKey(name))
                throw new InvalidOperationException("Dimension already exists");

            Dimension dimension = new Dimension(name);
            dimensions[name] = dimension;
            return dimension;
        }

        public Dimension GetDimension(string name)
        {
            if (dimensions.ContainsKey(name))
                return dimensions[name];

            return null;
        }

        public ICollection<Dimension> GetDimensions()
        {
            return dimensions.Values;
        }

        public IList<Value> AddTuple(IDictionary<string, object> values)
        {
            IList<Value> tuple = new List<Value>();

            foreach (var val in values)
            {
                Dimension dimension = this.GetDimension(val.Key);
                if (dimension == null)
                    dimension = this.CreateDimension(val.Key);
                tuple.Add(dimension.GetValue(val.Value));
            }

            return tuple;
        }
    }
}
