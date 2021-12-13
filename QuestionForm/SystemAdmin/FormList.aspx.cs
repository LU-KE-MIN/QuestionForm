using ActiveQuestion.Auth;
using ActiveQuestion.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionForm.SystemAdmin
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            Generate_Page();
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            

        }

        protected void Generate_Page()
        {
            
            if (!AuthManager.IsLogined())
            {
                Response.Redirect("/Login.aspx");
                return;
            }

            var currentUser = AuthManager.GetCurrentUser();

            if (currentUser == null)                             // 如果帳號不存在，導至登入頁
            {
                this.Session["UserLoginInfo"] = null;
                Response.Redirect("/Login.aspx");
                return;
            }

            
            // read accounting data
            var dt = QuestionnaireManager.GetQuestionnaireList();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["end_time"].ToString() != "")
                {
                    DateTime dbStart = Convert.ToDateTime(dt.Rows[i]["start_time"]);
                    DateTime dbEnd = Convert.ToDateTime(dt.Rows[i]["end_time"]);
                    DateTime timeNow = DateTime.Now;
                    int quesIDToClose = Convert.ToInt32(dt.Rows[i][0].ToString()); //找到對應的流水號
                    if ((timeNow - dbEnd).Days > 0)
                        QuestionnaireManager.CloseQuesStateByTime(quesIDToClose);
                    if ((timeNow - dbStart).Days < 0)
                        QuestionnaireManager.CloseQuesStateByTime(quesIDToClose);
                }
            }
            var dtPaged = this.GetPagedDataTable(dt);

            if (dt.Rows.Count == 0)
                this.ltlMsg.Text = "<br /><br /><br />查無資料";

            this.gvAccountingList.DataSource = dtPaged;

            if (!IsPostBack)
            {
                this.gvAccountingList.DataBind();
                this.ucPager.TotalSize = dt.Rows.Count;
                this.ucPager.Bind();
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var search = this.txtTitle.Text.Trim();
            dt = QuestionnaireManager.SearchQuestionnaire(search);
            

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DateTime dbStart = Convert.ToDateTime(dt.Rows[i]["start_time"]);
                DateTime dbEnd = Convert.ToDateTime(dt.Rows[i]["end_time"]);
                

                if (this.txtDateStart.Text != "" && this.txtDateEnd.Text != "") //開始結束不為空
                {
                    DateTime searchStart = Convert.ToDateTime(this.txtDateStart.Text);
                    DateTime searchEnd = Convert.ToDateTime(this.txtDateEnd.Text);
                    if ((searchEnd - searchStart).Days < 0)
                        Response.Write("<Script language='JavaScript'>alert('日期不可為負的'); location.href='FormList.aspx'; </Script>");
                    if ((dbEnd - searchStart).Days < 0 || (dbStart - searchEnd).Days > 0)//db資料庫內的 結束時間小於搜尋開始時間 OR 開始時間大於搜尋結束時間時 刪除此筆資料
                        dt.Rows[i].Delete();
                }
                else if (this.txtDateStart.Text != "" && this.txtDateEnd.Text == "") //只填開始時間
                {
                    DateTime searchStart = Convert.ToDateTime(this.txtDateStart.Text);
                    if ((dbEnd - searchStart).Days < 0)
                        dt.Rows[i].Delete();
                }
                else if (this.txtDateStart.Text == "" && this.txtDateEnd.Text != "") //只填結束時間
                {
                    DateTime searchEnd = Convert.ToDateTime(this.txtDateEnd.Text);
                    if ((dbStart - searchEnd).Days > 0)
                        dt.Rows[i].Delete();
                }
            }

            if (dt.Rows.Count == 0)
                this.ltlMsg.Text = "<br /><br /><br />查無資料";
            else
                this.ltlMsg.Text = "";

            this.gvAccountingList.DataSource = dt;
            this.gvAccountingList.DataBind();

            ucPager.Visible = false;
        }

        private DataTable GetPagedDataTable(DataTable dt)
        {
            DataTable dtPaged = dt.Clone();

            int startIndex = (this.GetCurrentPage() - 1) * 10;
            int endIndex = (this.GetCurrentPage()) * 10;
            if (endIndex > dt.Rows.Count)
                endIndex = dt.Rows.Count;

            for (var i = startIndex; i < endIndex; i++)
            {
                DataRow dr = dt.Rows[i];
                var drNew = dtPaged.NewRow();

                foreach (DataColumn dc in dt.Columns)
                {
                    drNew[dc.ColumnName] = dr[dc];
                }

                dtPaged.Rows.Add(drNew);
            }
            return dtPaged;
        }

        private int GetCurrentPage()
        {
            string pageText = Request.QueryString["Page"];

            if (string.IsNullOrWhiteSpace(pageText))
                return 1;
            int intPage;
            if (!int.TryParse(pageText, out intPage))
                return 1;
            if (intPage <= 0)
                return 1;
            return intPage;
        }
        protected void gvAccountingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lblState = row.FindControl("lblState") as Label;
                Label lblCaption = row.FindControl("lblCaption") as Label;
                Label lblStartDate = row.FindControl("lblStartDate") as Label;
                Label lblEndDate = row.FindControl("lblEndDate") as Label;

                var dr = row.DataItem as DataRowView;
                int QuesState = dr.Row.Field<int>("M_state");
                string QuesCaption = dr.Row.Field<string>("M_title");
                Guid QuesGuid = dr.Row.Field<Guid>("M_Guid");

                switch (QuesState)
                {
                    case 0:
                        lblState.Text = "已完結";
                        lblCaption.Text = QuesCaption;
                        break;
                    case 1:
                        lblState.Text = "投票中";
                        lblCaption.Text = $"<a href='FormPage.aspx?ID={QuesGuid}'>{QuesCaption}</a>";
                        break;
                }

                DateTime QuesStartDate = dr.Row.Field<DateTime>("start_time");
                if (QuesStartDate.ToString("yyyy-MM-dd") == "1800-01-01")
                    lblStartDate.Text = "-";
                else
                    lblStartDate.Text = QuesStartDate.ToString("yyyy-MM-dd");

                DateTime QuesEndDate = dr.Row.Field<DateTime>("end_time");
                if (QuesEndDate.ToString("yyyy-MM-dd") == "3000-12-31")
                    lblEndDate.Text = "-";
                else
                    lblEndDate.Text = QuesEndDate.ToString("yyyy-MM-dd");
            }
        
        }

    }
}