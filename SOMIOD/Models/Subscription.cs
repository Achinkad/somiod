using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Subscription
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_dt { get; set; }
        public int parent { get; set; }
        public string event_ { get; set; }

        public string endpoint { get; set; }

        public Subscription(int id, string name, DateTime creation_dt, int parent, string event_, string endpoint)
        {
            this.id = id;
            this.name = name;
            this.creation_dt = creation_dt;
            this.parent = parent;
            this.event_ = event_;
            this.endpoint = endpoint;
        }


    }
}