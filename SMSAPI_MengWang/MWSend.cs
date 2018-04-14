using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class MWSend
    {
        public static string myaccount = ConfigurationManager.AppSettings["MW_Sms_account"];
        public static string mypwd = ConfigurationManager.AppSettings["MW_Sms_pwd"];
        ///// <summary>
        ///// 固定字串
        ///// </summary>
        private const string FIX_STRING = "00000000";

        private int messageType = 0;
        private int authenticationMode = 0;
        private bool isKeepAlive = false;

        private const string content_VerificationCode = "您的验证码是{0}，在3分钟内输入有效。如非本人操作请忽略此短信。";

        private Account account = Account.getInstance();
        public void MWSendMsg(string phone,string code)
        {
            MWMessage msg = initMessage();
            msg.Mobile = phone;
            msg.Content = string.Format(content_VerificationCode, code);
            msg.ExNo = "";
            msg.CustId = "";
            msg.ExData = "";
            msg.SvrType = "";
            //提交
            string str =  submit(msg, 1);
        }


        /// <summary>
        /// 统一提交
        /// </summary>
        /// <param name="message">请求对象</param>
        /// <param name="sendType">请求类型,1:单发，2：相同内容群发，3：不同类型群发，4：获取上行，5：获取状态报告，6：获取账号余额</param>
        /// <returns></returns>
        private string submit(MWMessage message, int sendtype)
        {
            try
            {
                ISMS sms = null;
                if (messageType == 0)
                {
                    sms = new UrlEncdoeSend();
                }
                else if (messageType == 1)
                {
                    sms = new JsonSend();
                }
                else if (messageType == 2)
                {
                    //sms = new XMLSend();
                }
                account.Ip = "114.67.62.211";
                account.Port = 7901;
                account.MasterIPState = 0;
                String ipport = getIpPortByAccount(account);
                if (string.IsNullOrEmpty(ipport))
                {
                    return "没有可用的IP端口";
                }
                return sms.execute(message, sendtype, ipport, authenticationMode, this.isKeepAlive);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 初始发送对象
        /// </summary>
        /// <returns></returns>
        private MWMessage initMessage()
        {
            string imeStamp = DateTime.Now.ToString("MMddHHmmss");
            string password = encode(myaccount, "fG06bB", imeStamp);
            MWMessage msg = new MWMessage()
            {
                UserId = myaccount,
                TimeStamp = imeStamp,
                Pwd = password,
                ApiKey = ""
            };
            return msg;
        }

        /// <summary>
        /// 通过账号获取连接对象
        /// </summary>
        /// <param name="returnAccount"></param>
        /// <param name="userid"></param>
        /// <param name="connectionMap"></param>
        /// <returns></returns>
        public static string getIpPortByAccount(Account acc)
        {
            try
            {
                if (acc.MasterIPState == 0)
                    return acc.IpAndPort;
                else
                {
                    //checkMasterIpState(acc);
                    IList<IpAddress> list = acc.getIpAndPortBak();
                    foreach (IpAddress ia in list)
                    {
                        if (ia.Status)
                            return ia.IpAndPort;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("通过账号获取连接对象时发生异常,Error:{0}", ex.Message);
                return null;
            }
        }

        /**
        * 对密码进行加密
        * 
        * @description
        * @param userid
        *        账号
        * @param pwd
        *        原始密码
        * @param timestamp
        *        时间戳
        * @return 加密后的密码
        * @author JoNllen <jonllen.zn@qq.com>
        * @datetime 2016-9-1 下午01:40:55
        */
        public string encode(String userid, String pwd, String timestamp)
        {
            // 加密后的字符串 
            try
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile((userid + FIX_STRING + pwd + timestamp), "MD5").ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }
    }
}