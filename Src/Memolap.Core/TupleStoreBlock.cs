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
    }
}
