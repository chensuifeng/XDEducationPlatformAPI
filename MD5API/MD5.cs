using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.MD5API
{
    public class MD5 : IMessageEnCryption
    {
        private const string strkey = "01234567";
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="key">key</param>
        public string Decode(string key)
        {
            return EncrypExtends.DecryptDES(key, strkey);
        }
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="key">key</param>
        public string Encode(string key)
        {
            return EncrypExtends.EncryptDES(key, strkey);
        }


        /// <summary>
        /// DES加密解密 主调函数
        /// </summary>
        /// <param name="key">key</param>
        public string des(string msg)
        {
            return EncrypExtends.des(strkey, msg, false,false,string.Empty);
        }
    }
}