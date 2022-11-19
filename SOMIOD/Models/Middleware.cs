using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Middleware
    {
        private List<Application> apps { get; set; }

        public Middleware()
        {
            this.apps = new List<Application>();
        }
    }
}