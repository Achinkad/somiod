using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SOMIOD.Models
{
    public class Module : RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creation_dt { get; set; }
        public int Parent { get; set; }
        private List<Data> DataList { get; set; }
        private List<Subscription> Subscriptions { get; set; }
    }
}