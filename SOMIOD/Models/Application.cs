using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SOMIOD.Models
{
    public class Application
    {
        public int id { get; set; }

        public string name { get; set; }
        public DateTime creation_dt { get; set; }

        private List<Module> modules { get; set; }

        public Application()
        {
            this.id = id;
            this.name = name;
            this.creation_dt = creation_dt;
            this.modules = new List<Module>();
        }

    }
}