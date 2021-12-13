﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionForm
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 重新整理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReload_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.Request.RawUrl);
        }

        /// <summary>
        /// 返回列表頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToDefault_Click(object sender, EventArgs e)
        {
            Response.Redirect("../SystemAdmin/FormList.aspx");
        }

        /// <summary>
        /// 離開系統
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Login.aspx");
        }
    }
}