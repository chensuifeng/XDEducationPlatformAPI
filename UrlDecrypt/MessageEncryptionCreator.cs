﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.UrlDecrypt
{
    /// <summary>
    /// 加解密类获取器
    /// </summary>
    public static class MessageEncryptionCreator
    {
        private static Dictionary<string, IMessageEnCryption> msg = new Dictionary<string, IMessageEnCryption>();
        public static IMessageEnCryption GetInstance(string ver)
        {
            switch (ver)
            {
                case "1.0":
                    if (!msg.ContainsKey(ver))
                        lock (msg)
                            msg[ver] = new MessageEncryptionVersion1_0();

                    return msg[ver];
                case "1.1":
                    if (!msg.ContainsKey(ver))
                        lock (msg)
                            msg[ver] = new MessageEncryptionVersion1_1();

                    return msg[ver];
                default:
                    return null;
            }
        }
    }
}