using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using XDEducationPlatformAPI.common;
using System.Data;
using System.Text;
//using XDEducationPlatformAPI.SMSAPI_FeiGe;
using XDEducationPlatformAPI.SMSAPI_MengWang;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XDEducationPlatformAPI.Models;
using XDEducationPlatformAPI.SMSAPI_WngYi;

namespace XDEducationPlatformAPI.Controllers
{
    public class UserController : ApiController
    {
        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    bool isCorsRequest = request.Headers.Contains(Origin);
        //    bool isPreflightRequest = request.Method == HttpMethod.Options;
        //    if (isCorsRequest)
        //    {
        //        return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
        //        {
        //            HttpResponseMessage resp = t.Result;
        //            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
        //            return resp;
        //        });
        //    }
        //    else
        //    {
        //        return base.SendAsync(request, cancellationToken);
        //    }
        //}

        #region 测试接口
        [HttpGet]
        public string Hello()
        {
            return "Hello World!!";
        }

        [HttpGet]
        public string Hello1(string name, string id)
        {
            return "Hello,[" + name + "];id为[" + id + "];访问时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        [HttpPost]
        public object HelloTest([FromBody]JObject json)
        {
            return JsonConvert.DeserializeObject<LoginInfo>(JsonConvert.SerializeObject(json));
            //return  "Hello,[" + name + "];id为[" + id + "];访问时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"); ;
        }

        [HttpGet]
        public string GetString(string name)
        {
            return "hello:" + name + ";访问时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        #endregion


        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable UserRegister([FromBody]RegisterInfo json)
        {
            DataTable dt = null;

            try
            {
                EPData ep = new EPData();
                ValidInfo valid = ep.UserRegisterByCp(json);
                if (valid.valid)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("SELECT 'true' AS IsSuccess,'' AS Error,* FROM dbo.EP_User WHERE Phone = '{0}'", json.account);
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("IsSuccess", typeof(string));
                    dt.Columns.Add("Error", typeof(string));
                    DataRow dr = dt.NewRow();
                    dr["IsSuccess"] = valid.valid.ToString();
                    dr["Error"] = valid.errmsg;
                    dt.Rows.Add(dr);
                }

            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.Columns.Add("IsSuccess", typeof(string));
                dt.Columns.Add("Error", typeof(string));
                DataRow dr = dt.NewRow();
                dr["IsSuccess"] = "false";
                dr["Error"] = ex.Message;
                dt.Rows.Add(dr);
                Log.WriteError(ex, "注册（UserRegister）");
            }



            return dt;
        }
        #endregion


        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable UserLogin([FromBody]LoginInfo json)
        {
            DataTable dt = null;
            try
            {
                if (string.IsNullOrWhiteSpace(json.account))
                {
                    dt = new DataTable();
                    dt.Columns.Add("IsSuccess", typeof(string));
                    dt.Columns.Add("Error", typeof(string));
                    DataRow dr = dt.NewRow();
                    dr["IsSuccess"] = "false";
                    dr["Error"] = "账号错误";
                    dt.Rows.Add(dr);
                }
                else
                {
                    OperationData op = new OperationData();
                    dt = op.UserVerification(json.account, json.accounttype, json.password, json.code);
                }
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.Columns.Add("IsSuccess", typeof(string));
                dt.Columns.Add("Error", typeof(string));
                DataRow dr = dt.NewRow();
                dr["IsSuccess"] = "false";
                dr["Error"] = ex.Message;
                dt.Rows.Add(dr);
                Log.WriteError(ex, "登陆（UserLogin）");
            }
            return dt;
        }
        #endregion


        #region 发送验证码
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="json"></param>
        [HttpPost]
        public void SendCode([FromBody]CodeInfo json)
        {
            try
            {
                string phone = json.phone;
                string code = string.Empty;
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    Random rm = new Random();
                    code = rm.Next(100000, 1000000).ToString();
                    EPData ep = new EPData();
                    if (ep.InsertCodeByCP("cp_VerificationCodeInsert", phone, code,json.codetype))
                    {
                        //if (EPData.InsertCodeByCP("cp_VerificationCodeInsert", phone, code))
                        //{
                        //    Log.WriteCode(phone, code);
                        //}
                        //new MWSend().MWSendMsg(phone, code);
                        new WYSend().Send(phone, code);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, "发送验证码(SendCode)");
            }
        }
        #endregion


        #region 修改用户信息
        public DataTable UserModify([FromBody]UserModify user)
        {
            DataTable dt = null;
            try
            {
                OperationData op = new OperationData();
                dt = op.UserModify(user);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.Columns.Add("IsSuccess", typeof(string));
                dt.Columns.Add("Error", typeof(string));
                DataRow dr = dt.NewRow();
                dr["IsSuccess"] = "false";
                dr["Error"] = ex.Message;
                dt.Rows.Add(dr);
                Log.WriteError(ex, "修改用户信息(UserModify)");
            }
            return dt;
        }
        #endregion


        #region 我的收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public DataTable AddMyCollections([FromBody]MyCollections json)
        {
            DataTable dt = null;
            try
            {
                if (json.id > 0 && json.userid > 0)
                {
                    EPData ep = new EPData();
                    ValidInfo valid = ep.AddMyCollectionByCP(json);
                    if (valid.valid)
                    {
                        dt = GetTable("true", "");
                    }
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的收藏(SettMyCollections)");
            }
            return dt;
        }

        /// <summary>
        /// 删除我的收藏
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable DeleteMyCollections([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.collectionids))
                {
                    string colletionids = json.collectionids.Trim(',');
                    EPData ep = new EPData();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"DELETE dbo.EP_MyCollections WHERE UserId ={0} AND ID IN ({1})
                    BEGIN
                        IF (@@ROWCOUNT >=0)
	                    BEGIN
		                    SELECT 'true' AS IsSuccess,'' AS Error
	                    END
	                    ELSE
	                    BEGIN
	                        SELECT 'false' AS IsSuccess,'删除失败' AS Error
	                    END
                    END", json.userid, colletionids);
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的收藏(DeleteMyCollections)");
            }
            return dt;
        }



        /// <summary>
        /// 获取收藏
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable GetMyCollections([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0)
                {
                    EPData ep = new EPData();
                    dt = ep.GetData("SELECT * FROM EP_MyCollections WHERE UserId ={0} ORDER BY CreatTime DESC" + json.userid);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的收藏(GetMyCollection)");
            }

            return dt;
        }
        #endregion


        #region 我的消息
        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable GetMyMessages([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.isread))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("SELECT * FROM EP_MyMessages WHERE UserId={0} ", json.userid);
                    switch (json.isread)
                    {
                        case "0":
                            break;
                        case "1":
                            sb.Append(" AND IsRead=1");
                            break;
                        case "2":
                            sb.Append(" AND IsRead=0");
                            break;
                    }
                    sb.Append(" ORDER BY CreatTime DESC");
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的消息(GetMyMessages)");
            }
            return dt;
        }

        /// <summary>
        /// 设置我的消息已读
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable SetMyMessagesIsRead([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.msgids))
                {
                    string msgids = json.msgids.Trim(',');
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"UPDATE EP_MyMessages SET IsRead = 1 WHERE ID IN ({0})
                                    BEGIN
                                        IF(@@ROWCOUNT >= 0)
                                        BEGIN
                                            SELECT 'true' AS IsSuccess, '' AS Error
                                        END
                                        ELSE
                                        BEGIN
                                            SELECT 'false' AS IsSuccess, '设置已读失败' AS Error
                                        END
                                    END", msgids);
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的消息(SetMyMessagesIsRead)");
            }
            return dt;
        }

        /// <summary>
        /// 删除我的消息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable DeleteMyMessages([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.msgids))
                {
                    string msgids = json.msgids.Trim(',');
                    EPData ep = new EPData();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"DELETE dbo.EP_MyMessages WHERE UserId ={0} AND ID IN ({1})
                    BEGIN
                        IF (@@ROWCOUNT >=0)
	                    BEGIN
		                    SELECT 'true' AS IsSuccess,'' AS Error
	                    END
	                    ELSE
	                    BEGIN
	                        SELECT 'false' AS IsSuccess,'删除我的消息失败' AS Error
	                    END
                    END", json.userid, msgids);
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的消息(DeleteMyMessages)");
            }
            return dt;
        }

        /// <summary>
        /// 发送消息给用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public DataTable InsertMessagesToUser([FromBody]MyMessages json)
        {
            DataTable dt = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(json.userids))
                {
                    EPData ep = new EPData();
                    ValidInfo valid = ep.InsertMessagesToUserByCP(json);
                    dt = GetTable(valid.valid.ToString(), valid.errmsg);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的消息(SendMessagesToUser)");
            }
            return dt;
        }

        #endregion


        #region 我的笔记 
        /// <summary>
        /// 获取我的全部笔记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable GetMyNotes([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("SELECT * FROM EP_MyNotes WHERE UserId={0}", json.userid);
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的笔记(GetMyNotes)");
            }
            return dt;
        }

        /// <summary>
        /// 编辑我的笔记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable EditMyNotes([FromBody]MyNotes json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && json.id > 0)
                {
                    EPData ep = new EPData();
                    ep.EditMyNoteByCP(json);
                    dt = GetTable("true", "");
                    //SqlParameter[] sqlparam =null;
                    //sqlparam.(new SqlParameter() { });


                    //ep.EditMyNoteByCP("cp_EditMyNotes", sqlparam);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的笔记(EditMyNotes)");
            }
            return dt;
        }

        /// <summary>
        /// 删除笔记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable DeleteMyNotes([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.noteids))
                {
                    string noteids = json.noteids.Trim(',');
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"DELETE EP_MyNotes WHERE UserId={0} AND ID IN({1});
                    BEGIN
                        IF (@@ROWCOUNT >=0)
	                    BEGIN
		                    SELECT 'true' AS IsSuccess,'' AS Error
	                    END
	                    ELSE
	                    BEGIN
	                        SELECT 'false' AS IsSuccess,'删除我的笔记失败' AS Error
	                    END
                    END
                    ", json.userid, noteids);
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的笔记(DeleteMyNotes)");
            }

            return dt;
        }

        /// <summary>
        /// 查看笔记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable CheckMyNotes([FromBody]MyNotes json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && json.id > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("SELECT * FROM EP_MyNotes WHERE UserId={0} AND ID ={1}", json.userid, json.id);
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的笔记(CheckMyNotes)");
            }
            return dt;
        }

        /// <summary>
        /// 添加我的笔记
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>+
        [HttpPost]
        public DataTable AddMyNotes([FromBody]MyNotes json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0)
                {
                    EPData ep = new EPData();
                    ValidInfo valid = ep.AddMyNotesByCp(json);
                    dt = GetTable(valid.valid, valid.errmsg);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的笔记(AddMyNotes)");
            }
            return dt;
        }
        #endregion


        #region 我的收藏视屏
        /// <summary>
        /// 添加收藏视屏
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable AddMyCollectionVideos([FromBody]MyCollectionVideos json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0)
                {
                    OperationData od = new common.OperationData();
                    ValidInfo valid = od.AddMyCollectionVideos(json);
                    dt = GetTable(valid.valid, valid.errmsg);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的收藏视屏(AddMyCollectionVideos)");
            }
            return dt;
        }

        /// <summary>
        /// 删除收藏视屏
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable DeleteMyCollectionVideos([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.mycollectionivideoids))
                {
                    string videoids = json.mycollectionivideoids.Trim(',');
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"DELETE EP_MyCollectionVideos WHERE UserId ={0} AND ID IN({1});
                    BEGIN
                        IF (@@ROWCOUNT >=0)
	                    BEGIN
		                    SELECT 'true' AS IsSuccess,'' AS Error
	                    END
	                    ELSE
	                    BEGIN
	                        SELECT 'false' AS IsSuccess,'删除我的消息失败' AS Error
	                    END
                    END", json.userid, videoids);
                    EPData ep = new EPData();
                    dt = ep.GetData(sb.ToString());
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "我的收藏视屏(DeleteMyCollectionVideos)");
            }
            return dt;
        }

        /// <summary>
        /// 增加观看次数
        /// </summary>
        /// <param name="video"></param>
        [HttpPost]
        public void AddMyCollectionVideosWatchCounts([FromBody]MyCollectionVideos json)
        {
            try
            {
                if (json.userid > 0 && json.id > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"UPDATE EP_MyCollectionVideos SET WatchCounts = WatchCounts+1,MyWatchCounts =MyWatchCounts +1 WHERE UserId = {0} AND ID ={1};", json.userid, json.id);
                    EPData ep = new EPData();
                    ep.GetData(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex, "我的收藏视屏(AddMyCollectionVideosWatchCounts)");
            }
        }

        #endregion

        #region 密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable ResetMyPassword([FromBody]User json)
        {
            DataTable dt = null;
            try
            {
                if ((json.userid > 0 || !string.IsNullOrWhiteSpace(json.account)) && !string.IsNullOrWhiteSpace(json.password))
                {
                    OperationData od = new OperationData();
                    ValidInfo valid = od.ResetMyPasswordByCP(json);
                    dt = this.GetTable(valid.valid, valid.errmsg);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "修改密码(ResetMyPassword)");
            }
            return dt;
        }

        /// <summary>
        /// 验证码验证是否正确
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public DataTable VerificationCodeValid([FromBody]CodeInfo json)
        {
            DataTable dt = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(json.phone) && !string.IsNullOrWhiteSpace(json.code) && json.codetype>0)
                {
                    OperationData od = new OperationData();
                    ValidInfo valid = od.VerificationCodeValid(json);
                    dt = GetTable(valid.valid, valid.errmsg);
                }
                else
                {
                    dt = GetTable("false", "参数错误");
                }
            }
            catch (Exception ex)
            {
                dt = GetTable("false", ex.Message);
                Log.WriteError(ex, "验证码验证是否正确(VerificationCodeValid)");
            }
            return dt;
        }


        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        //[HttpPost]
        //public DataTable GetBackMyPassword([FromBody]User json)
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        if (json.userid > 0 && !string.IsNullOrWhiteSpace(json.password))
        //        {

        //        }
        //        else
        //        {
        //            dt = GetTable("false", "参数错误");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dt = GetTable("false", ex.Message);
        //        Log.WriteError(ex, "重置密码(GetBackPassword)");
        //    }
        //    return dt;
        //}
        #endregion

        #region 生成表格
        public DataTable GetTable(object valid, object msg)
        {
            DataTable dt = null;
            try
            {
                valid = valid == null ? string.Empty : valid.ToString().ToLower();
                msg = msg == null ? string.Empty : msg.ToString();
                dt = new DataTable();
                dt.Columns.Add("IsSuccess", typeof(string));
                dt.Columns.Add("Error", typeof(string));
                DataRow dr = dt.NewRow();
                dr["IsSuccess"] = valid;
                dr["Error"] = msg;
                dt.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        #endregion` 



    }

    /// <summary>
    /// 登陆参数
    /// </summary>
    public class LoginInfo
    {
        public string account { get; set; }
        public int accounttype { get; set; }
        public string password { get; set; }
        public string code { get; set; }
    }

}
