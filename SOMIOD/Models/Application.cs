using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SOMIOD.Models
{
    public class Application : RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Creation_dt { get; set; }
        private List<Module> Modules { get; set; }
    }
}