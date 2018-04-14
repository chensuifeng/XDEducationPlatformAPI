﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_FeiGe
{
    public class SendSmsResponse : BaseCode
    {
        /// <summary>
        /// 发送Id
        /// </summary>
        public string SendId { get; set; }
        /// <summary>
        /// 无效号码数量
        /// </summary>
        public int InvalidCount { get; set; }
        /// <summary>
        /// 成功数量
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 黑名单数量
        /// </summary>
        public int BlackCount { get; set; }
    }
}