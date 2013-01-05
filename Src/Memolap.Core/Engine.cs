namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private IDictionary<string, Dimension> dimensions = new Dictionary<string, Dimension>();
        private IList<Tuple> tuples = new List<Tuple>();

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

        public Tuple AddTuple(IDictionary<string, object> values)
        {
            var tuple = new Tuple(this, values);
            this.tuples.Add(tuple);

            return tuple;
        }

        public IEnumerable<Tuple> GetTuples(IDictionary<string, object> values)
        {
            foreach (var tuple in this.tuples)
                if (tuple.Match(values))
                    yield return tuple;
        }

        public int GetTupleCount()
        {
            return this.tuples.Count;
        }
    }
}
