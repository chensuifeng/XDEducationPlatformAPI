using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

namespace XDEducationPlatformAPI.common
{
    public class Log
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="strList"></param>
        public static void WriteLog(params object[] strList)
        {
            if (strList.Count() == 0) return;
            string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/log/logs/");
            string strFilePath = strDicPath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".txt";
            if (!Directory.Exists(strDicPath))
            {
                Directory.CreateDirectory(strDicPath);
            }
            if (!File.Exists(strFilePath))
            {
                using (FileStream fs = File.Create(strFilePath))
                {
                    fs.Close();
                }
                //File.Create(strFilePath);
            }
            //读取日志文件信息
            //string text = File.ReadAllText(strFilePath);

            StringBuilder sb = new StringBuilder();
            string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            sb.Append("\r\n日志记录：");
            foreach (var item in strList)
            {
                sb.Append("\r\n" +  item);
            }
            File.AppendAllText(strFilePath, strTime + ":" + sb.ToString() + "\r\n\r\n-------------------我是分割线----------------------------------------------------------------------------------\r\n\r\n\r\n");
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="DefFunc"></param>
        /// <param name="ErrorFunc"></param>
        //public static void WriteError(Action DefFunc, Func<string> ErrorFunc = null)
        //{
        //    try
        //    {
        //        DefFunc();
        //    }
        //    catch (Exception ex)
        //    {
        //        string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/log/error/");
        //        string strFilePath = strDicPath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        //        if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
        //        if (!File.Exists(strFilePath)) File.Create(strFilePath);

        //        //string text = File.ReadAllText(strFilePath);
        //        StringBuilder sb = new StringBuilder();
        //        if (ErrorFunc != null)
        //        {
        //            sb.Append("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:" + ErrorFunc());
        //        }
        //        sb.Append("\r\n"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")+ "------>:"+ ex.Message);
        //        sb.Append("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:" + ex.StackTrace);
        //        File.AppendAllText(strFilePath, sb.ToString() + "\r\n\r\n-------------------我是分割线----------------------------------------------------------------------------------\r\n\r\n\r\n");
        //    }
        //}

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex">错误</param>
        /// <param name="msg">描述</param>
        public static void WriteError(Exception ex,string msg =null)
        {
            string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/log/error/");
            string strFileName = strDicPath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!Directory.Exists(strDicPath)) Directory.CreateDirectory(strDicPath);
            if (!File.Exists(strFileName))
            {
                using (FileStream fs = File.Create(strFileName))
                {
                    fs.Close();
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("\r\n" +"错误日志记录"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:");
            if (ex != null)
            {
                sb.Append("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:" + ex.Message);
                sb.Append("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:" + ex.StackTrace);
            }
            if (msg != null)
            {
                sb.Append("\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "------>:" + msg);
            }
            File.AppendAllText(strFileName, sb.ToString() + "\r\n\r\n-------------------我是分割线----------------------------------------------------------------------------------\r\n\r\n\r\n");
        }


        /// <summary>
        /// 写入验证码
        /// </summary>
        /// <param name="strList"></param>
        public static void WriteCode(string phone,string code)
        {
            string strDicPath = System.Web.HttpContext.Current.Server.MapPath("~/log/codes/");
            string strFilePath = strDicPath + string.Format("{0:yyyy-MM-dd}", DateTime.Now) + ".txt";
            if (!Directory.Exists(strDicPath))
            {
                Directory.CreateDirectory(strDicPath);
            }
            if (!File.Exists(strFilePath))
            {
                using (FileStream fs = File.Create(strFilePath))
                {
                    fs.Close();
                }
                //File.Create(strFilePath);
            }
            //读取日志文件信息
            //string text = File.ReadAllText(strFilePath);

            StringBuilder sb = new StringBuilder();
            string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            sb.Append("\r\n日志记录：手机号：" + phone + ";插入验证码【" + code + "】成功");
            File.AppendAllText(strFilePath, strTime + ":" + sb.ToString() + "\r\n\r\n-------------------我是分割线----------------------------------------------------------------------------------\r\n\r\n\r\n");
        }
    }
}