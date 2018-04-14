using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class JsonMessageTemplate
    {
        /// <summary>
        /// 封装报文
        /// </summary>
        /// <param name="sendType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string getContentString(int sendType, MWMessage message, int authenticationMode)
        {
            if (sendType == 1)
                return getSingleContentString(message, authenticationMode);
            else if (sendType == 2)
                return getBatchContentString(message, authenticationMode);
            else if (sendType == 3)
                return getMultiMtContentString(message, authenticationMode);
            else if (sendType == 4)
                return getMoContentString(message, authenticationMode);
            else if (sendType == 5)
                return getRptContentString(message, authenticationMode);
            else if (sendType == 6)
                return getBalanceContentString(message, authenticationMode);

            return null;
        }
        /// <summary>
        /// 获取相同内群发封装UrlContent数据
        /// userid=kuke33&pwd=26dad7f364507df18f3841cc9c4ff94d&mobile=13800138000,13000130000,18000180000&content=%b2%e2%ca%d4%b6%cc%d0%c5&timestamp=0803192020&exno=0006&custid=b3d0a2783d31b21b8573&exdata=exdata000002
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string getSingleContentString(MWMessage message, int authenticationMode)
        {
            string contents = null;
            if (authenticationMode == 0)
            {
                contents = string.Format(TMP_SEND_CONTENT_JSON_NORMAL,
                                     message.UserId,
                                     message.Pwd,
                                     message.Mobile,
                                     HttpUtility.UrlEncode(message.Content, Encoding.GetEncoding("GBK")),
                                      message.TimeStamp,
                                    (string.IsNullOrEmpty(message.SvrType) ? string.Empty : message.SvrType),
                                    (string.IsNullOrEmpty(message.ExNo) ? string.Empty : message.ExNo),
                                    (string.IsNullOrEmpty(message.CustId) ? string.Empty : message.CustId),
                                    (string.IsNullOrEmpty(message.ExData) ? string.Empty : message.ExData));
            }
            else
            {
                contents = string.Format(TMP_SEND_CONTENT_JSON_APIKEY,
                       message.ApiKey,
                       message.Mobile,
                       HttpUtility.UrlEncode(message.Content, Encoding.GetEncoding("GBK")),
                       message.TimeStamp,
                        (string.IsNullOrEmpty(message.SvrType) ? string.Empty : message.SvrType),
                        (string.IsNullOrEmpty(message.ExNo) ? string.Empty : message.ExNo),
                        (string.IsNullOrEmpty(message.CustId) ? string.Empty : message.CustId),
                        (string.IsNullOrEmpty(message.ExData) ? string.Empty : message.ExData));

            }
            //两端加中括号
            contents = "{" + contents + "}";
            return contents;
        }
        /// <summary>
        /// 获取相同内群发封装UrlContent数据
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string getBatchContentString(MWMessage message, int authenticationMode)
        {
            return getSingleContentString(message, authenticationMode);
        }
        private static string getMultiMtContentString(MWMessage message, int authenticationMode)
        {
            string contents = null;

            if (authenticationMode == 0)
            {
                contents = string.Format(TMP_MULT_CONTENT_JSON_NORMAL,
                         message.UserId,
                         message.Pwd,
                         message.TimeStamp,
                         message.Multimt);
            }
            else
            {
                contents = string.Format(TMP_MULT_CONTENT_JSON_APIKEY,
                          message.ApiKey,
                          message.TimeStamp,
                          message.Multimt);

            }
            //两端加中括号
            contents = "{" + contents + "}";
            return contents;
        }


        private static string getMoContentString(MWMessage message, int authenticationMode)
        {
            string contents = null;

            if (authenticationMode == 0)
            {
                contents = string.Format(TMP_MO_JSON_NORMAL,
                                        message.UserId,
                                        message.Pwd,
                                        message.TimeStamp,
                                        message.RetSize);
            }
            else
            {
                contents = string.Format(TMP_MO_JSON_APIKEY,
                       message.ApiKey,
                       message.TimeStamp,
                       message.RetSize);

            }
            //两端加中括号
            contents = "{" + contents + "}";
            return contents;
        }

        private static string getRptContentString(MWMessage message, int authenticationMode)
        {
            string contents = null;

            if (authenticationMode == 0)
            {
                contents = string.Format(TMP_RPT_JSON_NORMAL,
                                message.UserId,
                                message.Pwd,
                                message.TimeStamp,
                                message.RetSize);
            }
            else
            {
                contents = string.Format(TMP_RPT_JSON_APIKEY,
                      message.ApiKey,
                      message.TimeStamp,
                      message.RetSize);
            }
            //两端加中括号
            contents = "{" + contents + "}";
            return contents;
        }

        private static string getBalanceContentString(MWMessage message, int authenticationMode)
        {
            string contents = null;
            if (authenticationMode == 0)
            {
                contents = string.Format(TMP_BALANCE_JSON_NORMAL,
                    message.UserId,
                    message.Pwd,
                    message.TimeStamp);
            }
            else
            {
                contents = string.Format(TMP_BALANCE_JSON_APIKEY,
                       message.ApiKey,
                      message.TimeStamp);
            }
            //两端加中括号
            contents = "{" + contents + "}";
            return contents;
        }


        #region 发送报文模板

        /// <summary>
        /// 发送报文JSON，默认账号+密码鉴权模板 
        /// </summary>
        public const string TMP_SEND_CONTENT_JSON_NORMAL = "\"userid\":\"{0}\",\"pwd\":\"{1}\",\"mobile\":\"{2}\",\"content\":\"{3}\",\"timestamp\":\"{4}\",\"svrtype\":\"{5}\",\"exno\":\"{6}\",\"custid\":\"{7}\",\"exdata\":\"{8}\"";

        /// <summary>
        /// 发送报文JSON， APIKEY鉴权模板 
        /// </summary>
        public const string TMP_SEND_CONTENT_JSON_APIKEY = "\"apikey\":\"{0}\",\"mobile\":\"{1}\",\"content\":\"{2}\",\"timestamp\":\"{3}\",\"svrtype\":\"{4}\",\"exno\":\"{5}\",\"custid\":\"{6}\",\"exdata\":\"{7}\"";



        #endregion

        #region 个性发送报文模板

        /// <summary>
        /// 个性发送报文JSON，默认账号+密码鉴权模板 
        /// </summary>
        public const string TMP_MULT_CONTENT_JSON_NORMAL = "\"userid\":\"{0}\",\"pwd\":\"{1}\",\"timestamp\":\"{2}\",\"multimt\":{3}";
        /// <summary>
        /// 个性发送报文JSON， APIKEY鉴权模板 
        /// </summary>
        public const string TMP_MULT_CONTENT_JSON_APIKEY = "\"apikey\":\"{0}\",\"timestamp\":\"{1}\",\"multimt\":{2}";

        #endregion

        #region 获取上行报文模板

        /// <summary>
        /// 获取上行报文JSON，默认账号+密码鉴权模板 
        /// </summary>
        public const string TMP_MO_JSON_NORMAL = "\"userid\":\"{0}\",\"pwd\":\"{1}\",\"timestamp\":\"{2}\",\"retsize\":{3}";
        /// <summary>
        /// 获取上行报文JSON， APIKEY鉴权模板 
        /// </summary>
        public const string TMP_MO_JSON_APIKEY = "\"apikey\":\"{0}\",\"timestamp\":\"{1}\",\"retsize\":{2}";

        #endregion

        #region 获取RPT报文模板
        /// <summary>
        /// 获取RPT报文JSON，默认账号+密码鉴权模板 
        /// </summary>
        public const string TMP_RPT_JSON_NORMAL = "\"userid\":\"{0}\",\"pwd\":\"{1}\",\"timestamp\":\"{2}\",\"retsize\":{3}";
        /// <summary>
        /// 获取RPT报文JSON， APIKEY鉴权模板 
        /// </summary>
        public const string TMP_RPT_JSON_APIKEY = "\"apikey\":\"{0}\",\"timestamp\":\"{1}\",\"retsize\":{2}";


        #endregion

        #region 获取查询余额报文模板

        /// <summary>
        /// 获取查询余额报文JSON，默认账号+密码鉴权模板 
        /// </summary>
        public const string TMP_BALANCE_JSON_NORMAL = "\"userid\":\"{0}\",\"pwd\":\"{1}\",\"timestamp\":\"{2}\"";
        /// <summary>
        /// 获取查询余额报文JSON， APIKEY鉴权模板 
        /// </summary>
        public const string TMP_BALANCE_JSON_APIKEY = "\"apikey\":\"{0}\",\"timestamp\":\"{1}\"";

        #endregion
    }
}