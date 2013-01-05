namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private IDictionary<string, Dimension> dimensions = new Dictionary<string, Dimension>();
        private IList<IList<Value>> tuples = new List<IList<Value>>();

        public Dimension CreateDimension(string name)
        {
            if (this.dimensions.ContainsKey(name))
                throw new InvalidOperationException("Dimension already exists");

            Dimension dimension = new Dimension(name);
            this.dimensions[name] = dimension;
            return dimension;
        }

        public Dimension GetDimension(string name)
        {
            if (this.dimensions.ContainsKey(name))
                return this.dimensions[name];

            return null;
        }

        public ICollection<Dimension> GetDimensions()
        {
            return this.dimensions.Values;
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

            this.tuples.Add(tuple);

            return tuple;
        }

        public int GetTupleCount()
        {
            return this.tuples.Count;
        }
    }
}
