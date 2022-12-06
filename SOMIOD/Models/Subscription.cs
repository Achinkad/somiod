using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Creation_dt { get; set; }
        public int Parent { get; set; }
        public string Event { get; set; }

        public string Endpoint { get; set; }

        public Subscription(int id, string name, DateTime creation_dt, int parent, string event_, string endpoint)
        {
            this.Id = id;
            this.Name = name;
            this.Creation_dt = creation_dt;
            this.Parent = parent;
            this.Event = event_;
            this.Endpoint = endpoint;
        }


    }
}