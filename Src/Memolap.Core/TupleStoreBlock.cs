namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TupleStoreBlock<T>
    {
        private ushort[] values;
        private T[] data;
        private int ndimensions;
        private int size;
        private int position;

        public TupleStoreBlock(int ndimensions)
            : this(ndimensions, 1024)
        {
        }

        public TupleStoreBlock(int ndimensions, int size)
        {
            this.ndimensions = ndimensions;
            this.size = size;
            this.values = new ushort[ndimensions * size];
            this.data = new T[size];
            this.position = 0;
        }

        public int Size { get { return this.size; } }

        public int Position { get { return this.position; } }

        public int NDimensions { get { return this.ndimensions; } }

        public ushort[] Values { get { return this.values; } }

        public T[] Data { get { return this.data; } }

        public void Add(ushort[] values, T data) 
        {
            int offset = this.position * this.ndimensions;

            for (int k = 0; k < this.ndimensions; k++)
                this.values[offset + k] = values[k];

            this.data[this.position] = data;

            this.position++;
        }
    }
}
