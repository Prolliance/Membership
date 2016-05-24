using Prolliance.Membership.ServicePoint.Lib;
using System;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr
{
    [SDK.Operation(TargetCode = "*", Code = "*")]
    public partial class Index : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //处理用户信息
            if (this.CurrentUser != null)
            {
                this.username.Text = this.CurrentUser.Name ?? this.CurrentUser.Account;
            }
            else if (this.CurrentState != null)
            {
                this.username.Text = this.CurrentState.Account;
            }
            //处理返回地址
            var backName = Convert.ToString(Session["back-name"] ?? "");
            var backUrl = Convert.ToString(Session["back-url"] ?? "");
            if (!string.IsNullOrWhiteSpace(backName) && !string.IsNullOrWhiteSpace(backUrl))
            {
                this.gobackArea.Visible = true;
                this.goback.Text = backName;
            }
            else
            {
                this.gobackArea.Visible = false;
            }
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentState != null)
                {
                    Business.User.CancelState(this.CurrentState.Token);
                }
            }
            catch
            {
                //不处理
            }
            this.CurrentState = null;
            this.PageEngine.GotoUrl("./login.aspx");
        }

        protected void goback_Click(object sender, EventArgs e)
        {
            var backUrl = Convert.ToString(Session["back-url"] ?? "");
            if (!string.IsNullOrWhiteSpace(backUrl))
            {
                this.PageEngine.GotoUrl(backUrl);
            }
            else
            {
                this.PageEngine.ShowMessageBox("返回地址异常");
            }
        }
    }
}