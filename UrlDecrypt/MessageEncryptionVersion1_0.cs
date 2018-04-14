using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.UrlDecrypt
{
    /// <summary>
    /// 加解密 只做 base64
    /// </summary>
    public class MessageEncryptionVersion1_0 : IMessageEnCryption
    {
        public string Decode(string content)
        {
            return content?.DecryptBase64();
        }

        public string Encode(string content)
        {
            return content.EncryptBase64();
        }
    }
}