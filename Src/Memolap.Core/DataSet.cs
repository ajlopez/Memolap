namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataSet<T> : ITupleStream<T>
    {
        private string name;
        private IList<Dimension> dimensions = new List<Dimension>();
        private IList<TupleObject<T>> tuples = new List<TupleObject<T>>();
        private IList<TupleStoreBlock<T>> blocks = new List<TupleStoreBlock<T>>();
        private int nblock = -1;

        public DataSet(string name) 
        {
            this.name = name;
        }

        public string Name { get { return this.name; } }

        public IList<Dimension> Dimensions { get { return this.dimensions; } }

        public IEnumerable<TupleObject<T>> Tuples 
        { 
            get 
            {
                int nblocks = this.blocks.Count;
                int ndimensions = this.dimensions.Count;
                TupleObject<T> tuple = new TupleObject<T>(this.dimensions);

                for (int nb = 0; nb < nblocks; nb++)
                {
                    TupleStoreBlock<T> block = this.blocks[nb];
                    int size = block.Position;

                    for (int k = 0; k < size; k++)
                    {
                        tuple.Adjust(block.Values, k * ndimensions, block.Data[k]);
                        yield return tuple;
                    }
                }
            } 
        }

        public Dimension CreateDimension(string name)
        {
            if (this.dimensions.Any(d => d.Name == name))
                throw new InvalidOperationException("Duplicated dimension");

            if (this.blocks.Count > 0)
                throw new InvalidOperationException("Dimension can not be created");

            Dimension dimension = new Dimension(name);
            this.dimensions.Add(dimension);
            return dimension;
        }

        public Dimension GetDimension(string name)
        {
            return this.dimensions.FirstOrDefault(d => d.Name == name);
        }

        public void AddData(IDictionary<string, object> values, T data)
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

            ushort[] vals = new ushort[this.dimensions.Count];

            k = 0;

            foreach (var key in keys)
            {
                int position = positions[k];
                Dimension dimension = this.dimensions[position];
                ushort val = dimension.GetValue(values[key]);
                vals[position] = val;
                k++;
            }

            if (this.nblock < 0 || this.blocks[this.nblock].Position >= this.blocks[this.nblock].Size)
            {
                this.nblock++;
                this.blocks.Add(new TupleStoreBlock<T>(this.dimensions.Count, 1024));
            }

            this.blocks[this.nblock].Add(vals, data);
        }

        public IQuery<T> Query()
        {
            return new BaseQuery<T>(this);
        }

        private int GetDimensionOffset(string name)
        {
            for (int k = 0; k < this.dimensions.Count; k++)
                if (this.dimensions[k].Name == name)
                    return k;

            return -1;
        }
    }
}
