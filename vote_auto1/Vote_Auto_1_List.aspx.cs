using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace vote_auto1
{
    public partial class Vote_Auto_1_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //-- 從資料表裡面撈出資料
            //-- 自動組成投票畫面(題目&子選項)
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ActiveQuestionConnectionString"].ConnectionString);
            Conn.Open(); //--連結DB

            SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand("select top 1 * from Vote_Auto order by vote_id DESC", Conn);
            //--把Vote_Auto 資料表裡面，最新的一場投票，呈現在網站的首頁上面。

            dr = cmd.ExecuteReader(); //-- 執行SQL指令。
            dr.Read();

            Label1.Text = "<b><font color=red>" + dr["title"].ToString() + "</font></b>";
            //--投票的主題(題目)

            int question_no = (int)dr["question_all"]; //--這次投票，共有幾個選項
            for(int i=1;i<= question_no; i++)
            {
                if(dr["question_"+i]== System.DBNull.Value)
                {
                    break; //--直接離開For迴圈。
                }
                else
                {
                    //--如果子選項裡面有「值」，就動態加入RadionButtonList的子選項
                    RadioButtonList1.Items.Add(dr["question_" + i].ToString());
                }
            }
            Session["vote_id"] = dr["vote_id"].ToString();
            //--下一隻程式會用到(Vote_Auto_2_End.aspx),展示得票數、長條圖)

            cmd.Cancel();
            dr.Close();
            Conn.Dispose();
        }
    }
}