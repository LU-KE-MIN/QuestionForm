using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

namespace vote_auto1
{
    public partial class Vote_Auto_0_Input : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlDataSource1.Insert();

            //==1). 將新的投票主題與候選人，寫入 Vote_Auto資料表。
        }

        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            int question_no = 0;

            //==2). 計算這次的投票題目，共有幾個選項?
            for(int i = 1; i <= 4; i++)
            {
                TextBox TB = (TextBox)Page.Form.FindControl("TextBox" + i);

                if (TB.Text == "")
                {
                    //--循序輸入「候選人」名稱,一旦發現名稱空白,表示結束(後面沒有其他候選人)
                    question_no = (i - 1);
                    break; //--完成，跳離for迴圈
                }
            }
            SqlConnection Conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["ActiveQuestionConnectionString"].ConnectionString);
            Conn.Open(); //----連結DB
            SqlCommand cmd = new SqlCommand("Update Vote_Auto set question_all =" + question_no + " where vote_id in (select top 1 vote_id from Vote_Auto order by vote_id DESC)", Conn);

            cmd.ExecuteNonQuery();
            cmd.Cancel();
            if(Conn.State == ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }
            Response.Redirect("Vote_Auto_1_List.aspx");
        }
    }
}