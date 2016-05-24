using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Prolliance.Membership.ServicePoint.JsApi
{
    /// <summary>
    /// TplHandler 的摘要说明
    /// </summary>
    public class TplHandler : IHttpHandler
    {
        public HttpContext Context { get; set; }

        public void ProcessRequest(HttpContext context)
        {
            this.Context = context;
            this.Context.Response.ContentType = "application/javascript";
            string tplName = this.Context.Request["name"] ?? "";
            this.Context.Response.Write(this.WrapTpl(this.ReadTplContent(tplName)));
            this.Context.Response.End();
        }

        private string WrapTpl(string content)
        {
            return string.Format("define('{0}');", content.Replace("'", "\\'").Replace("\r", "").Replace("\n", ""));
        }

        private static Dictionary<string, string> TplCache = new Dictionary<string, string>();
        private static object locker = new object();
        private string ReadTplContent(string name)
        {
            if (!TplCache.ContainsKey(name) || TplCache[name] == null)
            {
                lock (locker)
                {
                    locker = new object();
                    string filePath = string.Format("{0}/{1}.html",
                        this.Context.Server.MapPath("."), name);
                    TplCache[name] = File.ReadAllText(filePath);
                }
            }
            return TplCache[name];
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}