namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private IDictionary<string, Dimension> dimensions = new Dictionary<string, Dimension>();
        private IList<TupleObject> tuples = new List<TupleObject>();

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

        public void AddTuple(TupleObject tuple)
        {
            this.tuples.Add(tuple);
        }

        public TupleObject AddTuple(IDictionary<string, object> values)
        {
            var tuple = new TupleObject(this, values);
            this.AddTuple(tuple);

            return tuple;
        }

        public IEnumerable<TupleObject> GetTuples(IDictionary<string, object> values)
        {
            foreach (var tuple in this.tuples)
                if (tuple.Match(values))
                    yield return tuple;
        }

        public ICollection<Dimension> GetTuplesDimensions(IDictionary<string, object> values)
        {
            IList<Dimension> given = new List<Dimension>();

            foreach (var val in values) 
            {
                Dimension dimension = this.GetDimension(val.Key);
                if (dimension != null && !given.Contains(dimension))
                    given.Add(dimension);
            }

            IList<Dimension> dimensions = new List<Dimension>();

            foreach (var tuple in this.GetTuples(values))
                foreach (var value in tuple.GetValues())
                    if (!given.Contains(value.Dimension) && !dimensions.Contains(value.Dimension)) 
                    {
                        dimensions.Add(value.Dimension);
                        if (dimensions.Count >= this.dimensions.Count)
                            break;
                    }

            return dimensions;
        }

        public ICollection<Value> GetTuplesValues(IDictionary<string, object> values, string dimension)
        {
            IList<Value> vals = new List<Value>();

            foreach (var tuple in this.GetTuples(values))
            {
                Value value = tuple.GetValue(dimension);
                if (value != null && !vals.Contains(value))
                    vals.Add(value);
            }

            return vals;
        }

        public IDictionary<Value, object> MapReduceTuplesValues(IDictionary<string, object> values, string dimension, Func<TupleObject, object> newobj, Action<TupleObject, object> process)
        {
            IList<Value> vals = new List<Value>();
            IList<object> objects = new List<object>();

            foreach (var tuple in this.GetTuples(values))
            {
                Value value = tuple.GetValue(dimension);
                if (value == null)
                    continue;
                int index = vals.IndexOf(value);
                if (index >= 0)
                    process(tuple, objects[index]);
                else
                {
                    object obj = newobj(tuple);
                    vals.Add(value);
                    objects.Add(obj);
                    process(tuple, obj);
                }
            }

            IDictionary<Value, object> result = new Dictionary<Value, object>();

            for (var k = 0; k < vals.Count; k++)
                result[vals[k]] = objects[k];

            return result;
        }

        public int GetTupleCount()
        {
            return this.tuples.Count;
        }
    }
}
