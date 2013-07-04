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
    }
}
