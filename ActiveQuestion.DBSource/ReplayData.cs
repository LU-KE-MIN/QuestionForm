using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveQuestion.DBSource
{
    public class ReplyData
    {
        #region 回答與答題人資料

        /// <summary>
        /// 新增答題人
        /// </summary>
        /// <param name="UserGuid"></param>
        /// <param name="QuesGuid"></param>
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
                           (@userGuid
                           ,@quesGuid
                           ,@name
                           ,@phone
                           ,@email
                           ,@age
                            )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@userGuid", UserGuid));
            paramList.Add(new SqlParameter("@quesGuid", QuesGuid));
            paramList.Add(new SqlParameter("@name", Name));
            paramList.Add(new SqlParameter("@phone", Phone));
            paramList.Add(new SqlParameter("@email", Email));
            paramList.Add(new SqlParameter("@age", Age));

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
                $@"INSERT INTO [dbo].[User_Replay]
                               ([User_Guid]
                               ,[D_Guid]
                               ,[AnswerText])
                         VALUES
                    (
                        @userGuid
                       ,@probGuid
                       ,@answerText
                    )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@userGuid", UserGuid));
            paramList.Add(new SqlParameter("@probGuid", ProbGuid));
            paramList.Add(new SqlParameter("@answerText", AnswerText));

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
                      FROM [User_Replay]
                    WHERE [User_Guid] = @userGuid AND [D_Guid] = @probGuid
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@userGuid", UserGuid));
            list.Add(new SqlParameter("@probGuid", ProbGuid));
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
