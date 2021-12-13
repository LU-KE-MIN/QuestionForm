using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveQuestion.DBSource
{
    public class ProblemManager
    {
        #region 新增與修改問題

        /// <summary>
        /// 以 QuesGuid 取得問題資料表
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <returns></returns>
        public static DataTable GetProblem(Guid M_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT  [D_Guid]
                          ,[M_Guid]
                          ,[D_title]
                          ,[D_mustKeyin]
                          ,[SelectionType]
                          ,[Count]
                          ,[Selection]
                      FROM [ActiveQuestion].[dbo].[Q_d1]
                      WHERE [M_Guid]=@M_Guid
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

        /// <summary>
        /// 以 ProbGuid 取得該筆問題的內容
        /// </summary>
        /// <param name="D_Guid"></param>
        /// <returns></returns>
        public static DataRow GetProblemDataRow(Guid D_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT  [D_Guid]
                          ,[M_Guid]
                          ,[D_title]
                          ,[D_mustKeyin]
                          ,[SelectionType]
                          ,[Count]
                          ,[Selection]
                      FROM [ActiveQuestion].[dbo].[Q_d1]
                      WHERE [D_Guid]=@D_Guid
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@D_Guid", D_Guid));
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
        /// 以 QuesGuid 刪除所有問卷中的問題
        /// </summary>
        /// <param name="QuesGuid"></param>
        public static void DeleteProblemData(Guid D_Guid)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@" DELETE [Q_d1]
                    WHERE [D_Guid] = @D_Guid";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@D_Guid", D_Guid));

            try
            {
                DBHelper.ModifyData(connectionString, dbCommandString, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }

        /// <summary>
        /// 以 Session["ProblemDT"] 新增問題
        /// </summary>
        /// <param name="ProbGuid"></param>
        /// <param name="QuesGuid"></param>
        /// <param name="Count"></param>
        /// <param name="Text"></param>
        /// <param name="SelectionType"></param>
        /// <param name="IsMust"></param>
        /// <param name="Selection"></param>
        public static void CreateProblem(Guid D_Guid, Guid M_Guid, int Count, string D_title, int SelectionType, bool D_mustKeyin, string Selection)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@"INSERT INTO [Q_d1]
                               ([D_Guid]
                               ,[M_Guid]
                               ,[D_title]
                               ,[D_mustKeyin]
                               ,[SelectionType]
                               ,[Count]
                               ,[Selection])
                    VALUES
                    (
                        @D_Guid
                       ,@M_Guid
                       ,@D_title
                       ,@D_mustKeyin
                       ,@SelectionType
                       ,@Count
                       ,@Selection
                    )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@D_Guid", D_Guid));
            paramList.Add(new SqlParameter("@M_Guid", M_Guid));
            paramList.Add(new SqlParameter("@D_title", D_title));
            paramList.Add(new SqlParameter("@D_mustKeyin", D_mustKeyin));
            paramList.Add(new SqlParameter("@SelectionType", SelectionType));
            paramList.Add(new SqlParameter("@Count", Count));
            paramList.Add(new SqlParameter("@Selection", Selection));

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
        /// 新增問題後更新問卷的問題數
        /// </summary>
        /// <param name="QuesGuid"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool UpdateQuestionnaireCount(Guid D_Guid, int Count)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" UPDATE [Q_d1]
                    SET
                        [Count]    = @Count
                    WHERE
                        [D_Guid] = @D_Guid";

            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@D_Guid", D_Guid));
            paramList.Add(new SqlParameter("@Count", Count));

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
        #endregion

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
	                     ,[Ques].[Caption] AS 問卷名稱
	                     ,[Prob].[Text] AS 問題
	                     ,[Prob].[Selection] AS 問題選項
	                     ,[Ans].[AnswerText] AS 回答
	                     ,[User].CreateDate AS 填寫時間
                   FROM [Questionnaire] AS [Ques]
                     JOIN [ReplyInfo] AS [User] ON [Ques].QuesGuid = [User].QuesGuid
                     JOIN [Reply] AS [Ans] ON [User].UserGuid = [Ans].UserGuid
                     JOIN [Problem] AS [Prob] ON [Ans].ProbGuid = [Prob].ProbGuid
                   WHERE [Ques].QuesGuid = @quesGuid
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
