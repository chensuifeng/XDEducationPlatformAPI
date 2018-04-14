using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class MyCollections
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int collectiontype { get; set; }
        public string contentid { get; set; }
        public string contenturl { get; set; }
        public string imageurl { get; set; }
        public string title { get; set; }
        public string descriptions { get; set; }
        public bool deletelogic { get; set; }

    }
}