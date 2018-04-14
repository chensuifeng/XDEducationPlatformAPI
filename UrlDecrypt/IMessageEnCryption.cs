using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.UrlDecrypt
{
    /// <summary>
    /// 加密解密接口
    /// </summary>
    public interface IMessageEnCryption
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string Encode(string content);
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        string Decode(string content);
    }
}