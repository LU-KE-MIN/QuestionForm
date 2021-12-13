using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveQuestion.DBSource
{
    public class UserInfoManager
    {
        public static DataRow GetUserInfoByAccount(string account)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                @" SELECT 
                           [User_Guid]
                          ,[Account]
                          ,[Pwd]
                          ,[Level]
                          ,[Name]
                          ,[Phone]
                          ,[Email]
                          ,[Age]
              FROM [dbo].[UserInfo]
              WHERE Account=@account
                 ";


            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@account", account));

            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 列出使用者作答內容(填寫資料頁)
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <returns></returns>
        /// 

        /// <summary>
        /// 檢查同一問卷中手機是否重複
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool CheckPhoneIsRepeat(Guid M_Guid, string Phone)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT [Phone]
                    FROM [UserInfo]
                    WHERE [M_Guid] = @M_Guid AND [Phone] = @phone";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));
            paramList.Add(new SqlParameter("@phone", Phone));

            try
            {
                var dr = DBHelper.ReadDataRow(connStr, dbCommand, paramList);

                if (dr != null)
                {
                    var OrigPhone = dr[0].ToString();
                    if (Phone.Trim() == OrigPhone.Trim())
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 檢查同一問卷中Email是否重複
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        public static bool CheckEmailIsRepeat(Guid M_Guid, string Email)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT [Email]
                    FROM [UserInfo]
                    WHERE [M_Guid] = @M_Guid AND [Email] = @email";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));
            paramList.Add(new SqlParameter("@email", Email));

            try
            {
                var dr = DBHelper.ReadDataRow(connStr, dbCommand, paramList);

                if (dr != null)
                {
                    var OrigEmail = dr[0].ToString();
                    if (Email.Trim() == OrigEmail.Trim())
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }
        public static DataTable GetReplyInfo(Guid M_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT [User_Guid]
                          ,UserInfo.[M_Guid]
                          ,[Account]
                          ,[Pwd]
                          ,[Level]
                          ,[Name]
                          ,[Phone]
                          ,[Email]
                          ,[Age]
	                      ,[CreateDate]
                      FROM [ActiveQuestion].[dbo].[UserInfo]
                       INNER JOIN Q_M
                      ON UserInfo.M_Guid=Q_M.M_Guid
                    ORDER BY [CreateDate] DESC
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@M_Guid", M_Guid));
            try
            {
                return DBHelper.ReadDataTable(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        public static void DeleteReplyInfo(Guid M_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@" DELETE [UserInfo]
                    WHERE [M_Guid] = @M_Guid";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));

            try
            {
                DBHelper.ModifyData(connectionString, dbCommandString, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }


        #region 回答與答題人資料

        /// <summary>
        /// 新增答題人
        /// </summary>
        /// <param name="User_Guid"></param>
        /// <param name="M_Guid"></param>
        /// <param name="Name"></param>
        /// <param name="Phone"></param>
        /// <param name="Email"></param>
        /// <param name="Age"></param>
        public static void CreateReplyInfo(Guid UserGuid, Guid QuesGuid, string Name, string Phone, string Email, int Age)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" INSERT INTO [dbo].[UserInfo]
                                   ([User_Guid]
                                   ,[M_Guid]
                                   ,[Name]
                                   ,[Phone]
                                   ,[Email]
                                   ,[Age]
                                   )
                    VALUES
                    (
                        @User_Guid
                       ,@M_Guid
                       ,@Name
                       ,@Phone
                       ,@Email
                       ,@Age
                    )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@User_Guid", UserGuid));
            paramList.Add(new SqlParameter("@M_Guid", QuesGuid));
            paramList.Add(new SqlParameter("@Name", Name));
            paramList.Add(new SqlParameter("@Phone", Phone));
            paramList.Add(new SqlParameter("@Email", Email));
            paramList.Add(new SqlParameter("@Age", Age));

            try
            {
                DBHelper.CreatData(connStr, dbCommand, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// 以答題辨識碼來尋找答題人
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <returns></returns>
        public static DataRow GetReplyInfoDataRow(Guid UserGuid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT [User_Guid]
                          ,[M_Guid]
                          ,[Account]
                          ,[Pwd]
                          ,[Level]
                          ,[Name]
                          ,[Phone]
                          ,[Email]
                          ,[Age]
                          ,[CreateDate]
                      FROM UserInfo
                    WHERE [User_Guid] = @userGuid
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userGuid", UserGuid));
            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 新增回答
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="QuesGuid"></param>
        /// <param name="AnswerText"></param>
        public static void CreateReply(Guid UserGuid, Guid ProbGuid, string AnswerText)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" INSERT INTO User_Replay
                               ([User_Guid]
                               ,[D_Guid]
                               ,[AnswerText])
                         VALUES
                    (
                        @User_Guid
                       ,@D_Guid
                       ,@AnswerText
                    )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@User_Guid", UserGuid));
            paramList.Add(new SqlParameter("@D_Guid", ProbGuid));
            paramList.Add(new SqlParameter("@AnswerText", AnswerText));

            try
            {
                DBHelper.CreatData(connStr, dbCommand, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// 在填寫資料中印出那個填答人的回答
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="ProbGuid"></param>
        /// <returns></returns>
        public static DataRow GetReplyDataRow(Guid UserGuid, Guid ProbGuid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT [UserReplay_ID]
                          ,[User_Guid]
                          ,[D_Guid]
                          ,[AnswerText]
                      FROM User_Replay
                    WHERE [User_Guid] = @userGuid AND [D_Guid] = @D_Guid
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userGuid", UserGuid));
            list.Add(new SqlParameter("@D_Guid", ProbGuid));
            try
            {
                return DBHelper.ReadDataRow(connectionString, dbCommandString, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        #endregion
    }
}
