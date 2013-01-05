namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Dimension
    {
        public Dimension(string name)
        {
            this.Name = name;
            this.SetName = name + "s";
            if (name.EndsWith("y"))
                this.SetName = name.Substring(0, name.Length - 1) + "ies";
        }

        public string Name { get; set; }

        public string SetName { get; set; }
    }
}
