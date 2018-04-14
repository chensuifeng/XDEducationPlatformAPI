using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_FeiGe
{
    public class BaseCode
    {
        //状态码
        public int Code { get; set; }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string Message { get; set; }
    }
}