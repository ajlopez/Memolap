namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        public Dimension CreateDimension(string name)
        {
            Dimension dimension = new Dimension() { Name = name };
            return dimension;
        }
    }
}
