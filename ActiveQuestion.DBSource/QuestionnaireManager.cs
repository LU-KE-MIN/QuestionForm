using ActiveQuestion.DBSource;
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
    public class QuestionnaireManager
    {
        /// <summary> 查詢問卷清單 </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataTable GetQuestionnaireList()
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" SELECT [M_id]
                          ,[M_Guid]
                          ,[M_title]
                          ,[M_state]
                          ,[start_time]
                          ,[end_time]
                          ,[Summary]
                      FROM [dbo].[Q_M]
                      ORDER BY [M_id] DESC
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            

            try
            {
                return DBHelper.ReadDataTable(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 以 QuesGuid 取得問卷資料行
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <returns></returns>
        public static DataRow GetQuestionnaireDataRow(Guid M_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                    $@"SELECT [M_id]
                          ,[M_Guid]
                          ,[M_title]
                          ,[M_state]
                          ,[start_time]
                          ,[end_time]
                          ,[Count]
                          ,[Summary]
                      FROM [ActiveQuestion].[dbo].[Q_M]
                      WHERE [M_Guid] = @M_Guid
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@M_Guid", M_Guid));
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
        /// 因結束時間已到而關閉問卷
        /// </summary>
        /// <param name="M_id"></param>
        /// <returns></returns>
        public static bool CloseQuesStateByTime(int M_id)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" UPDATE [Q_M]
                    SET
                        [M_state] = 0
                    WHERE [M_id] = @M_id";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_id", M_id));

            try
            {
                int effectRows = DBHelper.ModifyData(connStr, dbCommand, paramList);

                if (effectRows == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 取得GUID來刪除選取的問卷
        /// </summary>
        /// <param name="M_id"></param>
        /// <returns></returns>
        public static DataRow GetM_GUIDForDeleteProblem(int M_id)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"DELETE 
                           Q_M
                          WHERE [M_id] =@M_id
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@M_id", M_id));

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
        /// 搜尋問卷
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        public static DataTable SearchQuestionnaire(string Search)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@" SELECT [M_id]
                          ,[M_Guid]
                          ,[M_title]
                          ,[M_state]
                          ,[start_time]
                          ,[end_time]
                          ,[Summary]
                           FROM [ActiveQuestion].[dbo].[Q_M]
                    WHERE [M_title] LIKE '%'+@search+'%'
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@search", Search));
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


        /// <summary> 查詢選取之問卷內容 </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static DataRow GetQuestionDetail(int list_id)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@"SELECT  [D_Guid]
                          ,[M_Guid]
                          ,[D_title]
                          ,[D_mustKeyin]
                          ,[SelectionType]
                          ,[Count]
                          ,[Selection]
                      FROM [dbo].[Q_d1]
                    WHERE M_id = @list_id ;"
;

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@list_id", list_id));
            

            try
            {
                return DBHelper.ReadDataRow(connStr, dbCommand, list);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }


        /// <summary>
        /// 新增問卷
        /// </summary>
        /// <param name="M_Guid"></param>
        /// <param name="M_title"></param>
        /// <param name="Summary"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="M_state"></param>
        /// <param name="Count"></param>
        public static void CreateQuestionnaire(Guid M_Guid, string M_title, string Summary, DateTime start_time, DateTime end_time, int M_state, int Count)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" INSERT INTO [dbo].[Q_M]
                                   (M_Guid
                                   ,M_title
                                   ,M_state
                                   ,start_time
                                   ,end_time
                                   ,Count
                                   ,Summary       
                                   )
                             VALUES
                                (
                                    @M_Guid
                                   ,@M_title
                                   ,@M_state
                                   ,@start_time
                                   ,@end_time
                                   ,@Count
                                   ,@Summary
                                )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));
            paramList.Add(new SqlParameter("@M_title", M_title));
            paramList.Add(new SqlParameter("@M_state", M_state));
            paramList.Add(new SqlParameter("@start_time", start_time));
            paramList.Add(new SqlParameter("@end_time", end_time));
            paramList.Add(new SqlParameter("@Count", Count));
            paramList.Add(new SqlParameter("@Summary", Summary));

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
        /// 修改問卷
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <param name="Caption"></param>
        /// <param name="Description"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="State"></param>
        /// <returns></returns>
        public static bool EditQuestionnaire(Guid M_Guid, string M_title, string Summary, DateTime start_time, DateTime end_time, int M_state)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" UPDATE [dbo].[Q_M]
                    SET
                        [M_title]     = @M_title
                       ,[Summary] = @Summary
                       ,[start_time]   = @start_time
                       ,[end_time]     = @end_time
                       ,[M_state]       = @M_state
                    WHERE
                        [M_Guid] = @M_Guid ";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));
            paramList.Add(new SqlParameter("@M_title", M_title));
            paramList.Add(new SqlParameter("@Summary", Summary));
            paramList.Add(new SqlParameter("@start_time", start_time));
            paramList.Add(new SqlParameter("@end_time", end_time));
            paramList.Add(new SqlParameter("@M_state", M_state));

            try
            {
                int effectRows = DBHelper.ModifyData(connStr, dbCommand, paramList);

                if (effectRows == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 從資料庫取得要匯出的 DataTable
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <returns></returns>
        public static DataTable OutputToCSV(Guid QuesGuid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT [User].[Name] AS 姓名
                         ,[User].[Phone] AS 電話
	                     ,[User].[Email] AS Email
	                     ,[User].[Age] AS 年齡
	                     ,[Ques].[M_title] AS 問卷名稱
	                     ,[Prob].[D_title] AS 問題
	                     ,[Prob].[Selection] AS 問題選項
	                     ,[Ans].[AnswerText] AS 回答
	                     ,[User].CreateDate AS 填寫時間
                   FROM [Q_M] AS [Ques]
                     JOIN [UserInfo] AS [User] ON [Ques].M_Guid = [User].M_Guid
                     JOIN [User_Replay] AS [Ans] ON [User].User_Guid = [Ans].User_Guid
                     JOIN [Q_d1] AS [Prob] ON [Ans].D_Guid = [Prob].D_Guid
                   WHERE [Ques].M_Guid = @quesGuid
                ";
            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@quesGuid", QuesGuid));
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

    }
}