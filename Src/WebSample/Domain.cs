namespace WebSample
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Memolap.Core;

    public class Domain
    {
        private static Domain instance;

        public Domain()
        {
            this.TupleSet = new TupleSet("Sample");
            this.TupleSet.CreateDimension("Category");
            this.TupleSet.CreateDimension("Country");
        }

        public static Domain Current
        {
            get
            {
                if (instance == null)
                    instance = new Domain();
                return instance;
            }
        }

        public TupleSet TupleSet { get; set; }

        public void InitializeFromFolder(string foldername)
        {
            this.LoadCategories(foldername);
            this.LoadCountries(foldername);
        }

        private void LoadCategories(string foldername)
        {
            string filename = Path.Combine(foldername, "Categories.txt");

            if (!File.Exists(filename))
                return;

            string[] categories = File.ReadAllLines(filename);
            var dimension = this.TupleSet.GetDimension("Category");

            foreach (var category in categories)
                dimension.GetValue(category);
        }

        private void LoadCountries(string foldername)
        {
            string filename = Path.Combine(foldername, "Countries.txt");

            if (!File.Exists(filename))
                return;

            string[] categories = File.ReadAllLines(filename);
            var dimension = this.TupleSet.GetDimension("Country");

            foreach (var category in categories)
                dimension.GetValue(category);
        }
    }
}