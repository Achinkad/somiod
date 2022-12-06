using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Creation_dt { get; set; }
        public int Parent { get; set; }

        public Data(int id, DateTime creation_dt, int parent)
        {
            this.Id = id;
            this.Creation_dt = creation_dt;
            this.Parent = parent;
        }


    }
}