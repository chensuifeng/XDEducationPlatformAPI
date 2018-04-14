using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class Account
    {
        //private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Account));
        /// <summary>
        /// 当前对象
        /// </summary>
        private static Account instance = null;
        /// <summary>
        /// 私有构造函数
        /// </summary>
        private Account()
        {
            if (instance != null)
                throw new Exception("实例化异常");
        }
        /// <summary>
        /// 获得单件实例MbossApplication
        /// </summary>
        /// <returns></returns>
        public static Account getInstance()
        {
            if (instance == null)
                instance = new Account();
            return instance;
        }

        /// <summary>
        /// 获取或设置账号
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        ///  获取或设置密码
        /// </summary> 
        public String PassWord { get; set; }



        /// <summary>
        /// 获取主IP端口信息 IP和端口号以:号连接
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
        private string ip;


        public string Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        private int port;

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        /// <summary>
        /// port
        /// </summary>
        public string DomainName { get; set; }

        /// <summary> 
        /// 获取或设置主IP状态 0正常 1异常
        /// </summary>
        public int MasterIPState { get; set; }


        /// <summary>
        /// 获取或设置备用IP端口信息 IP和端口号以:号连接
        /// </summary>
        private IList<IpAddress> ipAndPortBak = new List<IpAddress>();


        /// <summary>
        /// 备用IP端口信息 IP和端口号以:号连接
        /// </summary>
        /// <returns></returns>
        public IList<IpAddress> getIpAndPortBak()
        {
            return ipAndPortBak;
        }
        /// <summary>
        /// 设置备用IP端口信息 IP和端口号以:号连接
        /// </summary>
        /// <param name="ipAndPortBak"></param>
        public void setIpAndPortBak(IList<IpAddress> ipAndPortBak)
        {
            this.ipAndPortBak = ipAndPortBak;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipport"></param>
        /// <param name="state"></param>
        public void setState(String ipport, bool state)
        {
            //判断是否是主用
            if (ipport.Equals(IpAndPort))
            {
                MasterIPState = (state ? 0 : 1);
            }
            else
            {
                foreach (IpAddress ia in ipAndPortBak)
                {
                    if (ia.IpAndPort.Equals(ipport))
                    {
                        ia.Status = state;
                    }
                }
            }
        }
    }
}