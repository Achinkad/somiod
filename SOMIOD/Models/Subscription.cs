﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOMIOD.Models
{
    public class Subscription : RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creation_dt { get; set; }
        public int Parent { get; set; }
        public string Event { get; set; }
        public string Endpoint { get; set; }
    }
}