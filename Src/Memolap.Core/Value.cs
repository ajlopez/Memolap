namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Value
    {
        private Dimension dimension;
        private object obj;

        public Value(Dimension dimension, object obj)
        {
            this.dimension = dimension;
            this.obj = obj;
        }

        public Dimension Dimension { get { return this.dimension; } }

        public object Object { get { return this.obj; } }
    }
}
