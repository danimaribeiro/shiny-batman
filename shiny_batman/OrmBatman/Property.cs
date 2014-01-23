using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shiny_batman.OrmBatman
{
    public class Property
    {
        public bool IsPrimaryKey { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }

        public string RelatedModel { get; set; }
        public string RelatedColumn { get; set; }

        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }

        public object ActualValue { get; set; }
    }
}