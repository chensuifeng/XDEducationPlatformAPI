using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;

namespace XDEducationPlatformAPI.SMSAPI_FeiGe
{
    public class Send
    {
        public static string myaccount = ConfigurationManager.AppSettings["MW_Sms_account"];
        public static string mypwd = ConfigurationManager.AppSettings["MW_Sms_pwd"];
        public static string singid = ConfigurationManager.AppSettings["MW_Sms_singid"];
        public static void SendSms(string phone,string code)
        {
            string apiurl = "http://api.feige.ee";
            CommonSmsRequest request = new CommonSmsRequest
            {
                Account = myaccount,
                Pwd = mypwd,//登录web平台 http://sms.feige.ee  在管理中心--基本资料--接口密码 或者首页 接口秘钥 如登录密码修改，接口密码会发生改变，请及时修改程序
                Content = string.Format("您的验证码是:{0},请在3分钟内完成验证;如非本人操作请忽略此短信", code),
                Mobile = phone,
                SignId = Convert.ToInt32(singid), //登录web平台 http://sms.feige.ee  在签名管理中--新增签名--获取id
                SendTime = Convert.ToInt64(SMSCommon.ToUnixStamp(DateTime.Now))//定时短信 把时间转换成时间戳的格式
            };

            StringBuilder arge = new StringBuilder();
            arge.AppendFormat("Account={0}", request.Account);
            arge.AppendFormat("&Pwd={0}", request.Pwd);
            arge.AppendFormat("&Content={0}", request.Content);
            arge.AppendFormat("&Mobile={0}", request.Mobile);
            arge.AppendFormat("&SignId={0}", request.SignId);
            arge.AppendFormat("&SendTime={0}", request.SendTime);
            string weburl = apiurl + "/SmsService/Send";
            string resp = SMSCommon.PushToWeb(weburl, arge.ToString(), Encoding.UTF8);
            Console.WriteLine("SendSms:" + resp);
            try
            {
                SendSmsResponse response = JsonConvert.DeserializeObject<SendSmsResponse>(resp);
                if (response.Code == 0)
                {
                    //成功
                }
                else
                {
                    //失败
                }
            }
            catch (Exception ex)
            {
                //记录日志
                throw ex;
            }

        }
    }
}