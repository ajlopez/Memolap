namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private IDictionary<String, Dimension> dimensions = new Dictionary<String, Dimension>();

        public Dimension CreateDimension(string name)
        {
            if (dimensions.ContainsKey(name))
                throw new InvalidOperationException("Dimension already exists");

            Dimension dimension = new Dimension(name);
            dimensions[name] = dimension;
            return dimension;
        }

        public Dimension GetDimension(string name)
        {
            if (dimensions.ContainsKey(name))
                return dimensions[name];

            return null;
        }
    }
}
