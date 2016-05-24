using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint.Mgr
{
    [IgnoreSessionState]
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //处理返回信息
            this.HandleGoBackInfo();
            //处理登录信息
            var token = this.Request["token"] ?? "";
            var slot = this.Request["slot"] ?? "";
            if (!string.IsNullOrWhiteSpace(token))
            {
                this.LoginByToken(token);
            }
            else
            {
                this.RequestAuth();
            }
        }
        private void LoginByToken(string token)
        {
            this.CurrentState = Business.User.GetState(token);
            if (this.CurrentState != null)
            {
                var callback = Request["callback"] ?? "./";
                this.PageEngine.GotoUrl(callback);
            }
            else
            {
                this.PageEngine.ShowMessageBox("无效的 Token", "location.href='./login.aspx';");
            }
        }
        private string RequestAuthSlot
        {
            get
            {
                return Convert.ToString(Session["request_auto_slot"]);
            }
            set
            {
                Session["request_auto_slot"] = value;
            }
        }
        private void RequestAuth()
        {
            //this.RequestAuthSlot = StringFactory.NewGuid();
            //允许外部以 Token 跳入，不启用 Slot;
            var authUrl = string.Format("{0}?type={1}&scope={2}&appkey={3}&callback={4}",
                this.ResolveUrl("~/auth.aspx"),
                AuthType.SIMPLIFY,
                "all",
                AppSettings.Name,
                this.ResolveUrl("~/mgr/login.aspx"));
            this.PageEngine.GotoUrl(authUrl);
        }
        private void HandleGoBackInfo()
        {
            Session["back-name"] = this.Request["back-name"] ?? "";
            Session["back-url"] = this.Request["back-url"] ?? "";
        }
    }
}