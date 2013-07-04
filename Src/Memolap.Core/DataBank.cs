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

        public DataBank(string name) 
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public IList<Dimension> Dimensions { get { return this.dimensions; } }

        public Dimension AddDimension(string name)
        {
            Dimension dimension = new Dimension(name);
            this.dimensions.Add(dimension);
            return dimension;
        }

        public int GetDimensionOffset(string name)
        {
            for (int k = 0; k < this.dimensions.Count; k++)
                if (this.dimensions[k].Name == name)
                    return k;

            return -1;
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
                    this.AddDimension(key);
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

            return new TupleObject(vals, data);
        }
    }
}
