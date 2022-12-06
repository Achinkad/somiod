using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SOMIOD.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Creation_dt { get; set; }
        public int Parent { get; set; }

        private List<Data> DataList { get; set; }
        private List<Subscription> Subscriptions { get; set; }

        public Module(int id, string mname, DateTime creation_dt, int parent)
        {
            this.Id = id;
            this.Creation_dt = creation_dt;
            this.Parent = parent;
            this.DataList = new List<Data>();
            this.Subscriptions = new List<Subscription>();
        }
    }
}