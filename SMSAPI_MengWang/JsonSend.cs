using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
namespace XDEducationPlatformAPI.SMSAPI_MengWang
{
    public class JsonSend :ISMS
    {
        //private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(JsonSend));

        //请求路径
        private static String requestPath = "/sms/v2/std/";
        /// <summary>
        /// 提交请求
        /// </summary>
        /// <param name="message">请求报文</param>
        /// <param name="sendType">请求类型,1:单发，2：相同内容群发，3：不同类型群发，4：获取上行，5：获取状态报告，6：获取账号余额</param>
        /// <param name="IpAndPort">IP/PORT，格式:ip:port,例如：122.11.1.88:8888</param>
        /// <param name="authenticationMode">鉴权方式,0:账号+密码，1：APIKEY</param>
        /// <param name="bKeepAlive">是否长连接，TRUE：长连接，FALSE：短连接</param>
        /// <returns></returns>
        public string execute(MWMessage message, int sendType, string IpAndPort, int authenticationMode, bool bKeepAlive)
        {
            string url = getUrl(sendType, IpAndPort);
            string requestContent = JsonMessageTemplate.getContentString(sendType, message, authenticationMode);
            try
            {
                return httpPost(requestContent, url, bKeepAlive);
            }
            catch (Exception ex)
            {
                //log.ErrorFormat("发送发生异常,IP={0},error={1}", IpAndPort, ex.Message);
                //Account.getInstance().setState(IpAndPort, false);
                return ex.Message;
            }
        }

        /// <summary>
        /// 通过请求接口类型+IP/PORT获取完整的接口路径
        /// </summary>
        /// <param name="sendtype">请求类型,1:单发，2：相同内容群发，3：不同类型群发，4：获取上行，5：获取状态报告，6：获取账号余额</param>
        /// <param name="ipport">IP/PORT，格式:ip:port,例如：122.11.1.88:8888</param>
        /// <returns></returns>
        private string getUrl(int sendtype, string ipport)
        {
            string methodName = string.Empty;
            switch (sendtype)
            {
                case 1:
                    methodName = "single_send";
                    break;
                case 2:
                    methodName = "batch_send";
                    break;
                case 3:
                    methodName = "multi_send";
                    break;
                case 4:
                    methodName = "get_mo";
                    break;
                case 5:
                    methodName = "get_rpt";
                    break;
                case 6:
                    methodName = "get_balance";
                    break;
                default:
                    return null;
            }

            return string.Format("http://{0}{1}{2}", ipport, requestPath, methodName);
        }

        /// <summary>
        /// HTTP提交
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="contents"></param>
        /// <param name="bKeepAlive"></param>
        /// <returns></returns>
        private string httpPost(string contents, string uri, bool bKeepAlive)
        {
            //创建一个Http请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //设置请求的方法。
            request.Method = "POST";
            //设置 Content-typeHTTP 标头的值。 
            request.ContentType = "application/json";

            //请求所包含的 Connection HTTP 标头带有 Keep-alive 这一值，则为 true；否则为 false。默认值为  true。
            request.KeepAlive = bKeepAlive;
            //转为UTF-8
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(contents);
            //设置 Content-lengthHTTP 标头。
            request.ContentLength = bytes.Length;
            // 获取用于写入请求数据的 System.IO.Stream 对象。
            Stream os = request.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            //提供来自统一资源标识符 (URI) 的响应       
            System.Net.WebResponse response = request.GetResponse();
            if (response == null)
                return String.Empty;

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                //从流的当前位置到末尾读取流。
                return sr.ReadToEnd().Trim();
            }
        }
    }
}