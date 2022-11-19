using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Data
    {
        public int id { get; set; }
        public string content { get; set; }
        public DateTime creation_dt { get; set; }
        public int parent { get; set; }

        public Data(int id, DateTime creation_dt, int parent)
        {
            this.id = id;
            this.creation_dt = creation_dt;
            this.parent = parent;
        }


    }
}