using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class MyNotes
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int notettype { get; set; }
        public int notefromid { get; set; }
        public string notetitle { get; set; }
        public string notecontent { get; set; }
        public string noteurl { get; set; }
        public string notecontentdes { get; set; }
        public string noteimageurl { get; set; }
        public bool? deletelogic { get; set; }

    }
}