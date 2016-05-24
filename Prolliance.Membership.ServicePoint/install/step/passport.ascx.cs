using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class Passport : InstallControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormHelper.HideByLink(this, "dc");
            }
        }

        protected void next_Click(object sender, EventArgs e)
        {
            if (this.authType.SelectedValue.ToLower().Contains("dc") && (
                string.IsNullOrWhiteSpace(this.dc.Text)
                || string.IsNullOrWhiteSpace(this.administrator.Text)
                || string.IsNullOrWhiteSpace(this.password.Text)
                ))
            {
                this.AjaxPage.PageEngine.ShowMessageBox("参数设定错误");
                this.AjaxPage.PageEngine.UpdateControlRender(this);
                return;
            }
            InstallSettingGroup group = new InstallSettingGroup
            {
                Name = "passport",
                Value = this.authType.SelectedItem.Value
            };
            if (this.authType.SelectedValue.ToLower().Contains("dc"))
            {
                group.AddItem(new InstallSettingItem
                {
                    Name = "DC",
                    Value = this.dc.Text.Trim()
                });
                group.AddItem(new InstallSettingItem
                {
                    Name = "Administrator",
                    Value = this.administrator.Text.Trim()
                });
                group.AddItem(new InstallSettingItem
                {
                    Name = "Password",
                    Value = this.password.Text.Trim()
                });
            }
            InstallContext.AddGroup(group);
            this.InstallPage.NextStep();
        }

        protected void authType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.authType.SelectedValue.ToLower().Contains("dc"))
                FormHelper.ShowByLink(this, "dc");
            else
                FormHelper.HideByLink(this, "dc");
            this.AjaxPage.PageEngine.UpdateControlRender(this);
        }
    }
}