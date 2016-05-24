using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Linq;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class Account : InstallControlBase
    {
        public bool IsDC
        {
            get
            {
                var dcPassport = InstallContext.SettingGroups.FirstOrDefault(st => st.Name == "passport");
                var isDC = dcPassport != null && dcPassport.Value.ToLower().Contains("passport.dc");
                return isDC;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsDC)
            {
                FormHelper.HideByLink(this, "password");
            }
        }

        protected void next_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.account.Text)
                || (!IsDC
                    && (string.IsNullOrWhiteSpace(this.password.Text)
                        || this.password.Text.Trim().Length < 6)))
            {
                this.AjaxPage.PageEngine.ShowMessageBox("参数设定错误");
                this.AjaxPage.PageEngine.UpdateControlRender(this);
                return;
            }
#if !DEBUG
            try
            {
#endif
                InstallContext.Settings["account"] = this.account.Text.Trim();
                InstallContext.Settings["password"] = this.password.Text.Trim();
                InstallContext.Save();
                this.InstallPage.NextStep();
#if !DEBUG
            }
            catch (Exception ex)
            {
                this.AjaxPage.PageEngine.ShowMessageBox(ex.Message);
                this.AjaxPage.PageEngine.UpdateControlRender(this);
            }
#endif
        }
    }
}