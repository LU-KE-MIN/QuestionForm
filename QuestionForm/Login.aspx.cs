using ActiveQuestion.Auth;
using System;

namespace QuestionForm
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)//判斷頁面是不是第一次顯示
            {
                Session.RemoveAll();
                this.txtAccount.Text = "";
                this.txtPWD.Text = "";
            }

            if (this.Session["UserLoginInfo"] != null)
            {
                this.plcLogin.Visible = false;
                Response.Redirect("/SystemAdmin/FormList.aspx");
            }
            else
            {
                this.plcLogin.Visible = true;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string inp_Account = this.txtAccount.Text;
            string inp_PWD = this.txtPWD.Text;

            string msg;
            if (!AuthManager.TryLogin(inp_Account, inp_PWD, out msg))
            {
                this.ltlMsg.Text = msg;
                return;
            }

            if(this.Session["UserLoginInfoLevel"].ToString()=="0")
            Response.Redirect("/SystemAdmin/backFormList.aspx");

            if (this.Session["UserLoginInfoLevel"].ToString() == "1")
                Response.Redirect("/SystemAdmin/FormList.aspx");
        }
    }
}