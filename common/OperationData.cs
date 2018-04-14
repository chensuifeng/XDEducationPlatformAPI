using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using XDEducationPlatformAPI.Models;
using System.Data.SqlClient;

namespace XDEducationPlatformAPI.common
{
    public class OperationData
    {



        public void VerificationCodeInsert(string phone,string code)
        {
            try
            {
                int errint = 0;
                string errmsg = string.Empty;
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("EXEC cp_VerificationCodeInsert {0},{1},{2} OUTPUT,{3} OUTPUT;", phone, code, errint, errmsg);

                EPData ep = new EPData();
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }


        #region 用户验证
        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="account">登陆账号</param>
        /// <param name="accountType">登陆账号类型 1、账号登陆 2、手机登陆 </param>
        /// <param name="password">密码（手机登陆时 密码可以为空；用验证码验证）</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public DataTable UserVerification(string account, int accountType = 0, string password = null, string code = null)
        {
            DataTable dt = null;
            StringBuilder sb = new StringBuilder();
            try
            {


                sb.AppendFormat(@"DECLARE @account VARCHAR(100) = '{0}';
                                DECLARE @password VARCHAR(100)='{1}';
                                DECLARE @code VARCHAR(50) ='{2}';
                                DECLARE @accounttype INT = {3};
                                DECLARE @outvalid BIT =0;
                                DECLARE @outmsg VARCHAR(100);
                                BEGIN
                                    IF (@accounttype =1)   ----手机登陆
                                    BEGIN
                                        EXEC cp_EqualVerificationCode @account,@code,2,@outvalid OUTPUT,@outmsg OUTPUT;
                                    END
                                    ELSE IF (@accounttype =2)
                                    BEGIN
                                        EXEC cp_ValidPassword @account,@accounttype,@password,@outvalid OUTPUT,@outmsg OUTPUT;
                                    END
                                    ELSE
                                    BEGIN
                                        SELECT @outvalid AS IsSuccess, '账号类型不存在' AS Error;
                                        RETURN;
                                    END
                                    IF(@outvalid =1)
									    BEGIN
										    SELECT @outvalid AS IsSuccess, @outmsg AS Error,b.*,c.RoleName FROM EP_UserLogin AS a
										    INNER JOIN EP_User AS b ON a.UserID = b.ID
										    LEFT JOIN EP_UserRole AS c ON b.RoleId=c.ID WHERE a.LoginAccount=@account AND a.LoginType=@accounttype  AND a.DeleteLogic=0;
									    END
									ELSE
									    BEGIN
										    SELECT @outvalid AS IsSuccess, @outmsg AS Error;
									    END
                                END
                                ", account, password, code, accountType);


                EPData ep = new EPData();
                dt = ep.GetData(sb.ToString());
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("IsLogin", typeof(string));
                    dt.Columns.Add("Error", typeof(string));
                    DataRow dr = dt.NewRow();
                    dr["IsLogin"] = "false";
                    dr["Error"] = "UserVerification;dt为null";
                    dt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public DataTable UserModify(UserModify user)
        {
            DataTable dt  = null;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbwhere = new StringBuilder();
            StringBuilder sql = new StringBuilder();
            string phone = string.Empty;
            string idnumber = string.Empty;
            string id = string.Empty;
            try
            {
                if (user != null)
                {
                    foreach (var item in user.GetType().GetProperties())
                    {
                        string name = item.Name;
                        string value = item.GetValue(user) == null ? string.Empty : item.GetValue(user).ToString();
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (name == "phone")
                            {
                                phone = value;
                            }
                            if (name == "idnumber")
                            {
                                idnumber = value;
                            }
                            if (name == "id")
                            {
                                id = value;
                                sbwhere.AppendFormat(" WHERE {0}={1}", name, value);
                            }
                            else
                            {
                                if (sb != null && !string.IsNullOrWhiteSpace(sb.ToString()))
                                {
                                    sb.AppendFormat(" ,{0}='{1}'", name, value);
                                }
                                else
                                {
                                    sb.AppendFormat("{0}='{1}'", name, value);
                                }
                            }
                        }
                    }
                }
                if (sb != null && !string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    sql.Append("DECLARE @isexist INT =0;");
                    sql.Append("DECLARE @errmsg VARCHAR(500);");
                    if (!string.IsNullOrWhiteSpace(phone))       //手机不为空时 检查手机是否有重复
                    {
                        sql.AppendFormat(@"
                        IF EXISTS (SELECT 1 FROM EP_User WHERE Phone ='{0}' AND ID != {1} )
                        BEGIN
                            SET @isexist =1;
	                        SET @errmsg ='手机 ';
                        END", phone, id);
                    }
                    if (!string.IsNullOrWhiteSpace(idnumber))  //身份证不为空时 检查身份证是否有重复
                    {
                        sql.AppendFormat(@"
                        IF EXISTS (SELECT 1 FROM EP_User WHERE IDNumber ='{0}' AND ID != {1} )
                        BEGIN
                            SET @isexist =1;
	                        SET @errmsg = @errmsg + '身份证 ';
                        END", idnumber, id);
                    }
                    sql.AppendFormat(@"
                    IF (@isexist =1)
                    BEGIN
                        SELECT 'false' AS IsSuccess, @errmsg + '已存在' AS Error; 
                    END
                    ELSE
                    BEGIN
                        UPDATE EP_User SET {0} {1} ; 
	                    SELECT 'true' AS IsSuccess ,'' AS Error,* FROM EP_User {2};
                    END", sb.ToString(), sbwhere.ToString(), sbwhere.ToString());
                    EPData ep = new EPData();
                    dt = ep.GetData(sql.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }
        #endregion



        #region 添加视屏收藏
        /// <summary>
        /// 添加视屏收藏
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public ValidInfo AddMyCollectionVideos(MyCollectionVideos video)
        {
            ValidInfo valid = new ValidInfo();
            try
            {
                List<SqlParameter> sqllist = new List<SqlParameter>();

                sqllist.Add(new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.Int, Value = video.userid });
                sqllist.Add(new SqlParameter { ParameterName = "@videoid", SqlDbType = SqlDbType.Int, Value = video.videoid });

                sqllist.Add(new SqlParameter { ParameterName = "@videocategroyid", SqlDbType = SqlDbType.Int, Value = video.videocategroyid });
                sqllist.Add(new SqlParameter { ParameterName = "@videocategroy", SqlDbType = SqlDbType.VarChar, Value = video.videocategroy });
                sqllist.Add(new SqlParameter { ParameterName = "@videotitile", SqlDbType = SqlDbType.VarChar, Value = video.videotitile });
                sqllist.Add(new SqlParameter { ParameterName = "@videodes", SqlDbType = SqlDbType.VarChar, Value = video.videodes });
                sqllist.Add(new SqlParameter { ParameterName = "@videoimageurl", SqlDbType = SqlDbType.VarChar, Value = video.videoimageurl });
                sqllist.Add(new SqlParameter { ParameterName = "@videoprice", SqlDbType = SqlDbType.Decimal, Value = video.videoprice });
                sqllist.Add(new SqlParameter { ParameterName = "@videolecturerid", SqlDbType = SqlDbType.Int, Value = video.videolecturerid });
                sqllist.Add(new SqlParameter { ParameterName = "@videolecturer", SqlDbType = SqlDbType.VarChar, Value = video.videolecturer });
                sqllist.Add(new SqlParameter { ParameterName = "@issign", SqlDbType = SqlDbType.VarChar, Value = video.issign });

                sqllist.Add(new SqlParameter { ParameterName = "@watchcounts", SqlDbType = SqlDbType.Int, Value = video.watchcounts });
                sqllist.Add(new SqlParameter { ParameterName = "@mywatchcounts", SqlDbType = SqlDbType.Int, Value = video.mywatchcounts });

                sqllist.Add(new SqlParameter { ParameterName = "@outvalid", SqlDbType = SqlDbType.Bit,Direction = ParameterDirection.Output });
                sqllist.Add(new SqlParameter { ParameterName = "@outmsg", SqlDbType = SqlDbType.VarChar,Size=100,Direction = ParameterDirection.Output });   //错误提示：string[x]:size 属性具有无效大小值0  //解决方案： 输出参数需要明确指定长度

                EPData ep = new EPData();
                SqlCommand cmd = ep.CallProcCommon("cp_AddMyCollectionVideos", sqllist.ToArray());

                string outvalid = cmd.Parameters["@outvalid"].Value == null ? "" : cmd.Parameters["@outvalid"].Value.ToString().ToLower();
                string outmsg = cmd.Parameters["@outmsg"].Value == null ? "" : cmd.Parameters["@outmsg"].Value.ToString();
                cmd.Parameters.Clear();
                if (outvalid == "true" || outvalid == "True")
                {
                    valid.valid = true;
                    valid.errmsg = "";
                }
                else
                {
                    valid.valid = false;
                    valid.errmsg = outmsg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }
        #endregion

        #region 重置密码 
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ValidInfo ResetMyPasswordByCP(User user)
        {
            ValidInfo valid = new ValidInfo();
            try
            {
                List<SqlParameter> sqllist = new List<SqlParameter>();
                sqllist.Add(new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.Int, Value = user.userid });
                sqllist.Add(new SqlParameter { ParameterName = "@account", SqlDbType = SqlDbType.Int, Value = user.account });
                sqllist.Add(new SqlParameter { ParameterName = "@password", SqlDbType = SqlDbType.VarChar, Value = user.password });

                sqllist.Add(new SqlParameter { ParameterName = "@outvalid", SqlDbType = SqlDbType.Bit, Direction = ParameterDirection.Output });
                sqllist.Add(new SqlParameter { ParameterName = "@outmsg", SqlDbType = SqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });
                EPData ep = new common.EPData();
                SqlCommand cmd = ep.CallProcCommon("cp_ResetPassword", sqllist.ToArray());
                string outvalid = cmd.Parameters["@outvalid"].Value == null ? "" : cmd.Parameters["@outvalid"].Value.ToString().ToLower();
                string outmsg = cmd.Parameters["@outmsg"].Value == null ? "" : cmd.Parameters["@outmsg"].Value.ToString();
                cmd.Parameters.Clear();
                if (outvalid == "true" || outvalid == "True")
                {
                    valid.valid = true;
                    valid.errmsg = "";
                }
                else
                {
                    valid.valid = false;
                    valid.errmsg = outmsg;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }
        #endregion     

        #region 验证验证码是否正确
        /// <summary>
        /// 验证验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ValidInfo VerificationCodeValid(CodeInfo code)
        {
            ValidInfo valid = new ValidInfo();
            try
            {
                List<SqlParameter> sqllist = new List<SqlParameter>();
                sqllist.Add(new SqlParameter { ParameterName = "@phone", SqlDbType = SqlDbType.VarChar, Value = code.phone });
                sqllist.Add(new SqlParameter { ParameterName = "@code", SqlDbType = SqlDbType.VarChar, Value = code.code });
                sqllist.Add(new SqlParameter { ParameterName = "@codetype", SqlDbType = SqlDbType.Int, Value = code.codetype });
                sqllist.Add(new SqlParameter { ParameterName = "@outvalid", SqlDbType = SqlDbType.Bit, Direction= ParameterDirection.Output });
                sqllist.Add(new SqlParameter { ParameterName = "@outmsg", SqlDbType = SqlDbType.VarChar, Size = 100, Direction = ParameterDirection.Output });
                EPData ep = new EPData();
                SqlCommand cmd = ep.CallProcCommon("cp_EqualVerificationCode", sqllist.ToArray());
                string outvalid = cmd.Parameters["@outvalid"].Value == null ? "" : cmd.Parameters["@outvalid"].Value.ToString().ToLower();
                string outmsg = cmd.Parameters["@outmsg"].Value == null ? "" : cmd.Parameters["@outmsg"].Value.ToString();
                cmd.Parameters.Clear();
                if (outvalid == "true" || outvalid == "True")
                {
                    valid.valid = true;
                    valid.errmsg = "";
                }
                else
                {
                    valid.valid = false;
                    valid.errmsg = outmsg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valid;
        }
        #endregion

    }
}