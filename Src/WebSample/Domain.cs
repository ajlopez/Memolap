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
            this.Engine = new Engine();
            this.Engine.CreateDimension("Category");
            this.Engine.CreateDimension("Country");
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

        public Engine Engine { get; set; }

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
            var dimension = this.Engine.GetDimension("Category");

            foreach (var category in categories)
                dimension.GetValue(category);
        }

        private void LoadCountries(string foldername)
        {
            string filename = Path.Combine(foldername, "Countries.txt");

            if (!File.Exists(filename))
                return;

            string[] categories = File.ReadAllLines(filename);
            var dimension = this.Engine.GetDimension("Country");

            foreach (var category in categories)
                dimension.GetValue(category);
        }
    }
}