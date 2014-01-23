using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shiny_batman.OrmBatman
{
    public class BaseModel
    {
        public BaseModel()
        {
            InitializeProperties();
        }

        public string Name { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public List<Property> Properties {  get; set; }

        private void InitializeProperties()
        {
            this.Properties = new List<Property>();
        }
    }
}