namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Dimension
    {
        private IList<object> objects = new List<object>();
        private IList<Value> values = new List<Value>();

        public Dimension(string name)
        {
            this.Name = name;
            this.SetName = name + "s";
            if (name.EndsWith("y"))
                this.SetName = name.Substring(0, name.Length - 1) + "ies";
        }

        public string Name { get; set; }

        public string SetName { get; set; }

        public Value GetValue(object obj)
        {
            int index = this.objects.IndexOf(obj);

            if (index >= 0)
                return this.values[index];

            var newvalue = new Value(this, obj);
            this.objects.Add(obj);
            this.values.Add(newvalue);
            return newvalue;
        }

        public ICollection<Value> GetValues()
        {
            return this.values;
        }
    }
}
