using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class User
    {
        public int userid { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string account { get; set; }

        public int accounttype { get; set; }

        public string password { get; set; }
        /// <summary>
        /// 收藏id
        /// </summary>
        public string collectionids { get; set; }

        /// <summary>
        ///消息 0全部 1已读  2 未读
        /// </summary>
        public string isread { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        public string msgids { get; set; }

        /// <summary>
        /// 笔记id
        /// </summary>
        public string noteids { get; set; }

        /// <summary>
        /// 视屏收藏id
        /// </summary>
        public string mycollectionivideoids { get; set; }

    }
}