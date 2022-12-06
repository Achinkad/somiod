using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SOMIOD.Models
{
    public class Application
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime Creation_dt { get; set; }

        private List<Module> Modules { get; set; }

        public Application(int id, string name,DateTime creation_dt)
        {
            this.Id = id;
            this.Name = name;
            this.Creation_dt = creation_dt;
            this.Modules = new List<Module>();
        }

    }
}