using Prolliance.Membership.ServicePoint.Lib;
using System;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "user", Code = "password")]
    public partial class UserPassword : PageBase
    {
        protected Business.User Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var userId = this.PageEngine.GetWindowArgs<string>();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                this.Model = Business.User.GetUserById(userId);
                this.FillForm(this.Model);
            }
        }


        protected void save_Click(object sender, EventArgs e)
        {
            var pwd1 = this.ctl_Password1.Text.Trim();
            var pwd2 = this.ctl_Password2.Text.Trim();
            if (pwd1.Length < 6)
            {
                this.PageEngine.ShowMessageBox("密码设定不合要求");
                return;
            }
            if (pwd1 != pwd2)
            {
                this.PageEngine.ShowMessageBox("两次输入不一致");
                return;
            }
            try
            {
                if (this.Model != null)
                {
                    this.Model.SetPassword(pwd1);
                }
                this.PageEngine.CloseWindow();
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }
    }
}