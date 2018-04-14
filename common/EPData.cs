using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using XDEducationPlatformAPI.Models;
namespace XDEducationPlatformAPI.common
{
    public class EPData
    {
        private static string con = ConfigurationManager.ConnectionStrings["XDEducationPlatformAPIData"].ConnectionString;
        //private static SqlConnection sqlcon
        //{
        //    get { return new SqlConnection(con); }
        //}


        #region 获取数据
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public  DataTable GetData(string sql)
        {

            DataTable dt = null;
            DataSet ds = new DataSet();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {

                sqlcon.Open();

                SqlDataAdapter sqldata = new SqlDataAdapter(sql, sqlcon);
                sqldata.Fill(ds);
                dt = ds.Tables[0];
                sqlcon.Dispose();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                sqlcon.Dispose();
                sqlcon.Close();
                throw ex;
            }

            return dt;
        }
        #endregion



        #region 插入验证码
        /// <summary>
        /// 插入验证码
        /// </summary>
        /// <param name="sql">存储过程名</param>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public  bool InsertCodeByCP(string sql, string phone, string code,int codetype/*, *//*params SqlParameter[] paramters*/)
        {
            bool valid = false;
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {


                SqlCommand cmd = new SqlCommand(sql, sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@phone", SqlDbType.VarChar, 100).Value = phone;
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 100).Value = code;
                cmd.Parameters.Add("@codetype", SqlDbType.Int).Value = codetype;
                cmd.Parameters.Add("@errint", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@errmsg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                //cmd.Connection = sqlcon;
                //if (paramters != null)
                //{
                //    foreach (SqlParameter para in paramters)
                //    {
                //        cmd.Parameters.Add(para);
                //    }
                //}

                sqlcon.Open();
                cmd.ExecuteNonQuery();
                string errint = cmd.Parameters["@errint"].Value.ToString();
                string errmsg = cmd.Parameters["@errmsg"].Value.ToString();
                cmd.Parameters.Clear();
                sqlcon.Dispose();
                sqlcon.Close();
                if (errint == "0")
                {
                    valid = true;
                }
            }
            catch (Exception ex)
            {
                sqlcon.Dispose();
                sqlcon.Close();
                throw ex;
            }

            return valid;
        }
        #endregion



        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="info">注册信息</param>
        /// <returns></returns>
        public  ValidInfo UserRegisterByCp(RegisterInfo info)
        {
            ValidInfo valid = new ValidInfo();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                //string str = string.Empty;
                SqlCommand cmd = new SqlCommand("cp_RegisterUser", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@account", SqlDbType.VarChar, 100).Value = info.account;
                cmd.Parameters.Add("@accounttype", SqlDbType.Int).Value = info.accounttype;
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = info.password;
                cmd.Parameters.Add("@code", SqlDbType.VarChar, 100).Value = info.code;
                cmd.Parameters.Add("@outvalid", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@outerrormsg", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                sqlcon.Open();
                cmd.ExecuteNonQuery();
                string outvalid = cmd.Parameters["@outvalid"].Value.ToString();
                string errormsg = cmd.Parameters["@outerrormsg"].Value.ToString();
                //string valid = cmd.Parameters["@outvalid"].Value.ToString();
                //string errormsg = cmd.Parameters["@outerrormsg"].Value.ToString();
                //str = "{\"IsSuccess\":" + valid + ",\"ErrMsg\":" + errormsg + "}";
                cmd.Parameters.Clear();
                sqlcon.Dispose();
                sqlcon.Close();
                if (outvalid == "true" || outvalid == "True")
                {
                    valid.valid = true;
                    valid.errmsg = "";
                }
                else
                {
                    valid.valid = false;
                    valid.errmsg = errormsg;
                }

            }
            catch (Exception ex)
            {
                sqlcon.Dispose();
                sqlcon.Close();
                throw ex;
            }
            return valid;
        }
        #endregion



        #region 添加收藏
        public ValidInfo AddMyCollectionByCP(MyCollections coll)
        {
            ValidInfo valid = new ValidInfo();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                SqlCommand cmd = new SqlCommand("", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = coll.userid;
                cmd.Parameters.Add("@collectiontype", SqlDbType.Int).Value = coll.collectiontype;
                cmd.Parameters.Add("@contentid", SqlDbType.Int).Value = coll.contentid;
                cmd.Parameters.Add("@contenturl", SqlDbType.VarChar).Value = coll.contenturl;
                cmd.Parameters.Add("@imageurl", SqlDbType.VarChar).Value = coll.imageurl;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = coll.title;
                cmd.Parameters.Add("@descriptions", SqlDbType.VarChar).Value = coll.descriptions;
                cmd.Parameters.Add("@outvalid", SqlDbType.Bit).Direction = ParameterDirection.Output;
                sqlcon.Open();
                cmd.ExecuteNonQuery();
                sqlcon.Dispose();
                sqlcon.Close();
                string outvalid = cmd.Parameters["@outvalid"].Value == null ? "false" : cmd.Parameters["@outvalid"].Value.ToString().ToLower();
                if (outvalid == "true")
                {
                    valid.valid = true;
                }

            }
            catch (Exception ex)
            {
                sqlcon.Dispose();
                sqlcon.Close();
                throw ex;
            }

            return valid;
        }
        #endregion


        #region 插入消息
        /// <summary>
        /// 插入消息 存储过程
        /// </summary>
        /// <param name="mymsg"></param>
        /// <returns></returns>
        public ValidInfo InsertMessagesToUserByCP(MyMessages mymsg)
        {
            ValidInfo valid = new ValidInfo();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {

                SqlCommand cmd = new SqlCommand("cp_InsertMessagesToUser", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userids", SqlDbType.VarChar, 1000).Value = mymsg.userids;
                cmd.Parameters.Add("@msgtitle", SqlDbType.VarChar, 500).Value = mymsg.msgtitle;
                cmd.Parameters.Add("@msgcontent", SqlDbType.VarChar, 8000).Value = mymsg.msgcontent;
                cmd.Parameters.Add("@outvalid", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@outmsg", SqlDbType.VarChar).Direction = ParameterDirection.Output;
                sqlcon.Open();
                cmd.ExecuteNonQuery();
                sqlcon.Dispose();
                sqlcon.Close();
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
                sqlcon.Dispose();
                sqlcon.Close();
                throw ex;
            }
            return valid;
        }
        #endregion


        #region 编辑笔记
        public ValidInfo EditMyNoteByCP(MyNotes note)
        {
            ValidInfo valid = new ValidInfo();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                SqlCommand cmd = new SqlCommand("cp_EditMyNotes", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = note.id;
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = note.userid;
                cmd.Parameters.Add("@notetitle", SqlDbType.VarChar).Value = note.notetitle;
                cmd.Parameters.Add("@notecontent", SqlDbType.VarChar).Value = note.notecontent;
                cmd.Parameters.Add("@noteurl", SqlDbType.VarChar).Value = note.noteurl;
                cmd.Parameters.Add("@notecontentdes", SqlDbType.VarChar).Value = note.notecontentdes;
                cmd.Parameters.Add("@noteimageurl", SqlDbType.VarChar).Value = note.noteimageurl;
                //foreach (var item in note.GetType().GetProperties())
                //{
                //    object value = item.GetValue(note);
                //    if (value != null)
                //    {
                //        string param = "@" + item.Name.ToLower();
                //        cmd.Parameters.Add(param, GetSqlDbType(item.PropertyType.Name)).Value = value;
                //    }
                //}
                //cmd.Parameters.Add(sqlparams);
                sqlcon.Open();
                cmd.ExecuteNonQuery();
                SqlDisAndClose(sqlcon);
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                SqlDisAndClose(sqlcon);
                throw ex;
            }
            return valid;
        }
        #endregion

        #region 添加笔记
        public ValidInfo AddMyNotesByCp(MyNotes note)
        {
            ValidInfo valid = new ValidInfo();
            SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                SqlCommand cmd = new SqlCommand("cp_AddMyNotes", sqlcon);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userid", SqlDbType.Int).Value = note.userid;
                cmd.Parameters.Add("@notetype", SqlDbType.Int).Value = note.notettype;
                cmd.Parameters.Add("@notefromid", SqlDbType.Int).Value = note.notefromid;                                   
                cmd.Parameters.Add("@notetitle", SqlDbType.VarChar).Value = note.notetitle;
                cmd.Parameters.Add("@notecontent", SqlDbType.VarChar).Value = note.notecontent;
                cmd.Parameters.Add("@noteurl", SqlDbType.VarChar).Value = note.noteurl;
                cmd.Parameters.Add("@notecontentdes", SqlDbType.VarChar).Value = note.notecontentdes;
                cmd.Parameters.Add("@noteimageurl", SqlDbType.VarChar).Value = note.noteimageurl;
                cmd.Parameters.Add("@outvalid", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@outmsg", SqlDbType.VarChar).Direction = ParameterDirection.Output;

                sqlcon.Open();
                cmd.ExecuteNonQuery();
                SqlDisAndClose(sqlcon);
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
                SqlDisAndClose(sqlcon);
                throw ex;
            }
            return valid;
        }
        #endregion


        public SqlDbType GetSqlDbType(string propertyName)
        {
            SqlDbType sqltype = new SqlDbType();
            switch (propertyName.ToLower())
            {
                case "int32":
                    sqltype = SqlDbType.Int;
                    break;
                case "string":
                    sqltype = SqlDbType.VarChar;
                    break;
                case "bool":
                    sqltype = SqlDbType.Bit;
                    break;
            }
            return sqltype;
        }






        #region 释放资源 关闭数据连接
        public void SqlDisAndClose(SqlConnection sqlconn)
        {
            sqlconn.Dispose();
            sqlconn.Close();
        }
        #endregion


        #region 调用存储过程
        /// <summary>
        /// 条用存储过程
        /// </summary>
        /// <param name="procname">存储过程名</param>
        /// <param name="sqlary">参数</param>
        /// <returns></returns>
        public SqlCommand CallProcCommon(string procname ,params SqlParameter[] sqlary)
        {

            SqlConnection sqlcon = null;
            SqlCommand cmd = null;
            try
            {
                sqlcon = new SqlConnection(con);
                cmd = new SqlCommand(procname, sqlcon);
                foreach (var item in sqlary)
                {
                    if (item.Value == null)
                    {
                        item.Value = DBNull.Value;
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(sqlary);  ///传入SqlParameter[]时必须用AddRange 用Add会报 【不接受 SqlParameter[] 对象。】
                if (sqlcon.State == ConnectionState.Closed)
                {
                    sqlcon.Open();
                }
                cmd.ExecuteNonQuery();
                ///清除参数  清除参数后  cmd.Parameters[""]无法取到返回参数
                //cmd.Parameters.Clear();
                SqlDisAndClose(sqlcon);
            }
            catch (Exception ex)
            {
                SqlDisAndClose(sqlcon);
                throw ex; 
            }

            return cmd;

        }
        #endregion


    }
}