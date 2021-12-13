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
    public partial class FormConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = this.Request.QueryString["ID"];

            if (string.IsNullOrWhiteSpace(id) || id.Length != 36)
            {
                Response.Write("<Script language='JavaScript'>alert(' QueryString 錯誤，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");
                return;
            }

            DataRow QuesRow = QuestionnaireManager.GetQuestionnaireDataRow(Guid.Parse(id));  // 問卷
            DataTable ProblemDT = ProblemManager.GetProblem(Guid.Parse(id)); // 問題

            if (QuesRow == null || QuesRow["M_Guid"].ToString() != id)
            {
                Response.Write("<Script language='JavaScript'>alert(' Guid 錯誤，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");
                return;
            }

            DateTime startDate = DateTime.Parse(QuesRow["start_time"].ToString()); // 帶入開始日期
            string StartString = startDate.ToString("yyyy/MM/dd");
            if (StartString != "1800/01/01")
                this.lblDuring.Text += StartString;
            else
                this.lblDuring.Text += "";

            this.lblDuring.Text += " ~ "; // 開始日期與結束日期間的分隔

            DateTime endDate = DateTime.Parse(QuesRow["end_time"].ToString()); // 帶入結束日期
            string EndString = endDate.ToString("yyyy/MM/dd");
            if (EndString != "3000/12/31")
                this.lblDuring.Text += EndString;
            else
                this.lblDuring.Text += "";

            // 判斷問卷日期是否過期
            DateTime nowDate = DateTime.Now;
            if ((nowDate - startDate).Days < 0 || (nowDate - endDate).Days > 0)
                Response.Write("<Script language='JavaScript'>alert('此問卷已經過期，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");

            // 判斷問卷狀態是否關閉
            if (Convert.ToInt32(QuesRow["M_state"]) == 0)
                Response.Write("<Script language='JavaScript'>alert('此問卷已經關閉，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");

            this.lblCaption.Text = QuesRow["M_title"].ToString(); // 帶入標題
            this.lblDescription.Text = QuesRow["Summary"].ToString(); // 帶入描述

            // 從確認頁返回問卷填寫頁修改資料時回填
            if (Session["Name"] != null)
                this.lblNameValue.Text = Session["Name"].ToString();
            if (Session["Phone"] != null)
                this.lblPhoneValue.Text = Session["Phone"].ToString();
            if (Session["Email"] != null)
                this.lblEmailValue.Text = Session["Email"].ToString();
            if (Session["Age"] != null)
                this.lblAgeValue.Text = Session["Age"].ToString();

            if (ProblemDT.Rows.Count > 0)
            {
                if (Session["Reply"] != null)
                {
                    string[] reply = Session["Reply"].ToString().Split(';');
                    for (int i = 0; i < ProblemDT.Rows.Count; i++)
                        this.ltlReply.Text += $"<p>{i + 1}. {ProblemDT.Rows[i]["D_title"]} <br /> &nbsp &nbsp {reply[i]}</p><br />"; // 印出作答的內容以供確認
                }
            }
        }

        /// <summary>
        /// 返回問卷填寫頁進行修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditC_Click(object sender, EventArgs e)
        {
            string id = this.Request.QueryString["ID"];
            Response.Redirect("FormPage.aspx?ID=" + id);
        }

        /// <summary>
        /// 送出回答並寫到資料庫 >> 新增 ReplyInfo、Reply 及增加單選複選的統計
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSendC_Click(object sender, EventArgs e)
        {
            string id = this.Request.QueryString["ID"];
            Guid idToGuid = Guid.Parse(id);
            DataTable ProblemDT = ProblemManager.GetProblem(idToGuid); // 問題

            // 將個人基本資料輸入 ReplyInfo 資料表
            Guid UserGuid = Guid.NewGuid();
            //Guid QuesGuid = idToGuid;
            string Name = Session["Name"].ToString();
            string Phone = Session["Phone"].ToString();
            string Email = Session["Email"].ToString();
            int Age = Convert.ToInt32(Session["Age"]);

            // 新增答題人
            UserInfoManager.CreateReplyInfo(UserGuid, idToGuid, Name, Phone, Email, Age);

            // 以答題辨識碼來尋找答題人
            //ReplyData.GetReplyInfoDataRow(UserGuid);

            if (Session["Reply"] != null)
            {
                string[] answerText = Session["Reply"].ToString().Split(';'); // 取出以分號分割的回答

                for (int i = 0; i < ProblemDT.Rows.Count; i++) // 每一題
                {
                    //Guid QuesGuid = Guid.Parse(ProblemDT.Rows[i]["QuesGuid"].ToString());
                    Guid ProbGuid = Guid.Parse(ProblemDT.Rows[i]["D_Guid"].ToString());
                    int SelectionType = Convert.ToInt32(ProblemDT.Rows[i]["SelectionType"]);

                    // 新增回答
                    UserInfoManager.CreateReply(UserGuid, ProbGuid, answerText[i]);

                    // 若單選或複選被打勾，則將統計資料表中對應的 Count + 1
                    if (SelectionType == 0 && !string.IsNullOrWhiteSpace(answerText[i]))
                    {
                        StaticData.UpdateStaticCount(ProbGuid, answerText[i]); // 單選統計 + 1
                    }

                    if (SelectionType == 1 && !string.IsNullOrWhiteSpace(answerText[i]))
                    {
                        string[] checkBoxAns = answerText[i].Split(','); // 再取出以逗號分割的值
                        for (int j = 0; j < checkBoxAns.Length; j++)
                            StaticData.UpdateStaticCount(ProbGuid, checkBoxAns[j]); // 複選統計 + 1
                    }
                }
            }
            Response.Write($"<Script language='JavaScript'>alert('問卷已送出，感謝您的填寫!!'); location.href='FormStatic.aspx?ID={id}'; </Script>");
            return;
        }
    }
}