using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveQuestion.DBSource
{
    public class CommonProblem
    {
        /// <summary>
        /// 取得問卷
        /// </summary>
        /// <returns></returns>
        public static DataTable GetFAQ()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@" SELECT [FAQID]
                          ,[Count]
                          ,[Name]
                          ,[Text]
                          ,[SelectionType]
                          ,[IsMust]
                          ,[Option]
                      FROM [ActiveQuestion].[dbo].[Common]
                      ORDER BY [FAQID] ASC
                ";
            List<SqlParameter> list = new List<SqlParameter>();

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
        /// 以 CommID 取得該筆常用問題的內容
        /// </summary>
        /// <param name="CommID"></param>
        /// <returns></returns>
        public static DataRow GetFAQByFAQID(int FAQID)
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@"SELECT [FAQID]
                          ,[Count]
                          ,[Name]
                          ,[Text]
                          ,[SelectionType]
                          ,[IsMust]
                          ,[Option]
                      FROM [ActiveQuestion].[dbo].[Common]
                       WHERE FAQID = @FAQID
                ";

            List<SqlParameter> list = new List<SqlParameter>();
            list.Add(new SqlParameter("@FAQID", FAQID));

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
        /// 刪除常用問題中所有內容
        /// </summary>
        public static void DeleteCommonData()
        {
            string connectionString = DBHelper.GetConnectionString();
            string dbCommandString =
                $@" DELETE [Common]";

            List<SqlParameter> paramList = new List<SqlParameter>();

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
        /// 以 Session["CommonDT"] 進資料庫新增常用問題
        /// </summary>
        /// <param name="Count"></param>
        /// <param name="Name"></param>
        /// <param name="Text"></param>
        /// <param name="SelectionType"></param>
        /// <param name="IsMust"></param>
        /// <param name="Selection"></param>
        public static void CreateCommon(int Count, string Name, string Text, int SelectionType, bool IsMust, string Option)
        {
            string connStr = DBHelper.GetConnectionString();
            string dbCommand =
                $@" INSERT INTO [Common]
                    (
                          [Count]
                        , [Name]
                        , [Text]
                        , [SelectionType]
                        , [IsMust]
                        , [Option]
                    )
                    VALUES
                    (
                        @count
                       ,@name
                       ,@text
                       ,@selectionType
                       ,@isMust
                       ,@option
                    )
                ";
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@count", Count));
            paramList.Add(new SqlParameter("@name", Name));
            paramList.Add(new SqlParameter("@text", Text));
            paramList.Add(new SqlParameter("@selectionType", SelectionType));
            paramList.Add(new SqlParameter("@isMust", IsMust));
            paramList.Add(new SqlParameter("@option", Option));

            try
            {
                DBHelper.CreatData(connStr, dbCommand, paramList);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
    }
}
