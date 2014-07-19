﻿namespace Memolap.Core
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
            return this.objects[position];
        }

        public int GetValue(object obj)
        {
            int index = this.objects.IndexOf(obj);

            if (index >= 0)
                return index;

            this.objects.Add(obj);
            return this.objects.Count - 1;
        }

        public ICollection<object> GetValues()
        {
            return this.objects;
        }
    }
}
