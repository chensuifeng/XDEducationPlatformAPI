using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.Models
{
    public class RegisterInfo
    {
        /// <summary>
        /// 手机/账号
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 账号类型 1、手机 2、普通账号 3、身份证  4、微信
        /// </summary>
        public int accounttype { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string code { get; set; }
    }
}