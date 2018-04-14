using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.UrlDecrypt
{
    /// <summary>
    /// 数据加解密 des
    /// </summary>
    public class MessageEncryptionVersion1_1 : IMessageEnCryption
    {
        //public static readonly string KEY = "fHil/4]0";
        public static readonly string KEY = "0123456789ABCDEF";
        public string Decode(string content)
        {
            return content.DecryptDES(KEY);
        }

        public string Encode(string content)
        {
            return content.EncryptDES(KEY);
        }
    }
}