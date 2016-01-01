namespace Memolap.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Dimension
    {
        private IList<object> objects = new List<object>();

        public Dimension(string name)
        {
            this.Name = name;
            this.SetName = name + "s";
            if (name.EndsWith("y"))
                this.SetName = name.Substring(0, name.Length - 1) + "ies";
        }

        public string Name { get; set; }

        public string SetName { get; set; }

        public object GetValue(int position)
        {
            if (position == 0)
                return null;

            return this.objects[position - 1];
        }

        public ushort GetValue(object obj)
        {
            if (obj == null)
                return 0;

            int index = this.objects.IndexOf(obj);

            if (index >= 0)
                return (ushort)(index + 1);

            this.objects.Add(obj);
            return (ushort)this.objects.Count;
        }

        public ICollection<object> GetValues()
        {
            return this.objects;
        }
    }
}
