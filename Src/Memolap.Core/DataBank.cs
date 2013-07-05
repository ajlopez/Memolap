namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataBank
    {
        private string name;
        private IList<Dimension> dimensions = new List<Dimension>();
        private IList<TupleObject> tuples = new List<TupleObject>();

        public DataBank(string name) 
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public IList<Dimension> Dimensions { get { return this.dimensions; } }

        public Dimension CreateDimension(string name)
        {
            if (this.dimensions.Any(d => d.Name == name))
                throw new InvalidOperationException("Duplicated dimension");

            Dimension dimension = new Dimension(name);
            this.dimensions.Add(dimension);
            return dimension;
        }

        public Dimension GetDimension(string name)
        {
            return this.dimensions.FirstOrDefault(d => d.Name == name);
        }

        public int GetDimensionOffset(string name)
        {
            for (int k = 0; k < this.dimensions.Count; k++)
                if (this.dimensions[k].Name == name)
                    return k;

            return -1;
        }

        public TupleObject CreateTuple()
        {
            TupleObject tuple = new TupleObject(this);
            this.tuples.Add(tuple);
            return tuple;
        }

        public TupleObject CreateTuple(IDictionary<string, object> values, object data)
        {
            int[] positions = new int[values.Count];
            IList<string> keys = values.Keys.ToList();
            int k = 0;

            foreach (var key in keys)
            {
                int position = this.GetDimensionOffset(key);

                if (position < 0)
                {
                    position = this.dimensions.Count;
                    this.CreateDimension(key);
                }

                positions[k++] = position;
            }

            Value[] vals = new Value[this.dimensions.Count];

            k = 0;

            foreach (var key in keys)
            {
                int position = positions[k];
                Dimension dimension = this.dimensions[position];
                Value val = dimension.GetValue(values[key]);
                vals[position] = val;
                k++;
            }

            var tuple = new TupleObject(this, vals, data);

            this.tuples.Add(tuple);

            return tuple;
        }

        public IEnumerable<TupleObject> GetTuples(IDictionary<string, object> values)
        {
            foreach (var tuple in this.tuples)
                if (tuple.Match(values))
                    yield return tuple;
        }

        public ICollection<object> GetTuplesValues(IDictionary<string, object> values, string dimension)
        {
            IList<object> vals = new List<object>();

            foreach (var tuple in this.GetTuples(values))
            {
                object value = tuple.GetValue(dimension);
                if (value != null && !vals.Contains(value))
                    vals.Add(value);
            }

            return vals;
        }

        public IDictionary<object, object> MapReduceTuplesValues(IDictionary<string, object> values, string dimension, Func<TupleObject, object> newobj, Action<TupleObject, object> process)
        {
            IList<object> vals = new List<object>();
            IList<object> objects = new List<object>();

            foreach (var tuple in this.GetTuples(values))
            {
                object value = tuple.GetValue(dimension);
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

            IDictionary<object, object> result = new Dictionary<object, object>();

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
