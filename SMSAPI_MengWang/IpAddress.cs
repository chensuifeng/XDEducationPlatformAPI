using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class IpAddress
    {
        /// <summary>
        /// 获取或设置主IP端口信息 IP和端口号以:号连接
        /// </summary>
        public String IpAndPort
        {
            get
            {
                if (String.IsNullOrEmpty(ip))
                    return null;
                return ip + ":" + port;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private string ip;

        /// <summary>
        /// 
        /// </summary>
        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }
        private int port;

        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private string domainName;

        /// <summary>
        /// 
        /// </summary>
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        private DateTime lastTime = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastTime
        {
            get { return lastTime; }
            set { lastTime = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private bool status = true;

        /// <summary>
        /// 
        /// </summary>
        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}