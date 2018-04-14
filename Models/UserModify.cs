using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class UserModify
    {
        public int id { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string idnumber { get; set; }

        public string sex { get; set; }

        public string personalimageurl { get; set; }

        public string birthday { get; set; }

        public string address { get; set; }

        private DateTime? _modifytime;
        public DateTime? modifytime
        {
            get
            {
                return DateTime.Now;
            }
            set
            {
                _modifytime = value;
            }
        }
        public int? modifyuser { get; set; }
    }
}