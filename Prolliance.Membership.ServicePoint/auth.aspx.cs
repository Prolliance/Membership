using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint
{
    [IgnoreSessionState]
    public partial class Auth : PageBase
    {
        public string AuthType
        {
            get
            {
                return Request["type"] ?? Request["authtype"] ?? "";
            }
        }
        public string AppKey
        {
            get
            {
                return Request["appkey"] ?? Request["client_id"] ?? "";
            }
        }
        public string RedirectUrl
        {
            get
            {
                return Request["redirect"] ?? Request["callback"] ?? "";
            }
        }
        public string Scope
        {
            get
            {
                return Request["scope"] ?? "";
            }
        }
        public string Slot
        {
            get
            {
                return Request["slot"] ?? "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.CheckParameter();
            }
        }

        private bool CheckParameter()
        {
            if (this.AuthType != Business.AuthType.SIMPLIFY
                    || string.IsNullOrWhiteSpace(this.AppKey)
                    || this.Scope != "all"
                    || string.IsNullOrWhiteSpace(this.RedirectUrl))
            {
                this.PageEngine.ShowMessageBox("参数错误");
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void loginButton_Click(object sender, EventArgs e)
        {
            if (this.CheckParameter())
            {
                try
                {
                    Business.AuthParameter parameter = new Business.AuthParameter();
                    parameter.Type = Business.AuthType.PASSWORD;
                    parameter.AppKey = this.AppKey;
                    parameter.Account = this.accountBox.Text;
                    parameter.Password = this.passwordBox.Text;
                    this.LoginByParameter(parameter);
                }
                catch (Exception ex)
                {
                    this.PageEngine.ShowMessageBox(ex.Message);
                }
            }
        }

        private void LoginByParameter(AuthParameter parameter)
        {
            var userState = Business.User.CreateState(parameter);
            if (userState != null && !string.IsNullOrWhiteSpace(userState.Token))
            {
                string url = this.RedirectUrl + (this.RedirectUrl.Contains("?") ? "&" : "?") + "token=" + userState.Token;
                url += "&slot=" + this.Slot;
                this.PageEngine.GotoUrl(url);
            }
            else
            {
                this.PageEngine.ShowMessageBox("账号或密码错误，也可能账号已被禁用。");
            }
        }

    }
}