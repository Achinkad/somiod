using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Module
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime creation_dt { get; set; }
        public int parent { get; set; }

        private List<Data> dataList { get; set; }
        private List<Subscription> subscriptions { get; set; }

        public Module(int id, string mname, DateTime creation_dt, int parent)
        {
            this.id = id;
            this.creation_dt = creation_dt;
            this.parent = parent;
            this.dataList = new List<Data>();
            this.subscriptions = new List<Subscription>();
        }
    }
}