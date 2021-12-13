using ActiveQuestion.DBSource;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QuestionForm.Handlers
{
    /// <summary>
    /// StaticHandler 的摘要描述
    /// </summary>
    public class StaticHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string id = context.Request.QueryString["M_Guid"];
            if (string.IsNullOrEmpty(id))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("required");
                context.Response.End();
            }

            Guid idToGuid = Guid.Parse(id);
            DataTable dt = StaticData.DrawStatic(Guid.Parse(id));
            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(dt);

            context.Response.ContentType = "application/json";
            context.Response.Write(jsonText);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}