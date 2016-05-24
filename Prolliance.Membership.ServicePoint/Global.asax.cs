
using Prolliance.Membership.Common;
using Prolliance.Membership.Install.Lib;
using System;
using System.Reflection;

namespace Prolliance.Membership.ServicePoint
{
    public class Global : System.Web.HttpApplication
    {

#if DISTRIBUTION
    
#endif

        private static string _Version { get; set; }
        public static string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_Version))
                {
                    _Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                return _Version;
            }
        }

        private Logger logger = new Logger(AppSettings.LogPath);

        protected void Application_Start(object sender, EventArgs e)
        {
            logger.Info("服务已启动。");
            InstallContext.AppPhysicsPath = Server.MapPath("~/");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
//#if DISTRIBUTION
//            if (!licenseClient.Validate(Server.MapPath("~/bin/license.key")))
//            {
//                Context.Response.Clear();
//                Context.Response.Write(string.Format("未授权,机器序列号: {0}", licenseClient.GenMachineSerialNumber()));
//                Context.Response.End();
//            }
//#endif
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            logger.Error(ex);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            logger.Info("服务已停止。");
        }
    }
}