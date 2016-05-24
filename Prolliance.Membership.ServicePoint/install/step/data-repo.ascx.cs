using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class DataRepo : InstallControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormHelper.HideByLink(this, "el");
                FormHelper.ShowByLink(this, "sqlserver");
            }
        }

        protected void next_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.connString.Text))
            {
                this.AjaxPage.PageEngine.ShowMessageBox("参数设定错误");
                this.AjaxPage.PageEngine.UpdateControlRender(this);
                return;
            }
            InstallSettingGroup group = new InstallSettingGroup
            {
                Name = "data-provider",
                Value = this.dataRepoType.SelectedItem.Value
            };
            group.AddItem(new InstallSettingItem
            {
                Name = "ConnectionString",
                Value = this.connString.Text.Trim()
            });
            InstallContext.AddGroup(group);
            this.InstallPage.NextStep();
        }

        protected void dataRepoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormHelper.HideByLink(this, "el");
            var showLinkId = this.dataRepoType.SelectedValue.ToLower().Contains("mongodb") ? "mongodb" : "sqlserver";
            FormHelper.ShowByLink(this, showLinkId);
            this.AjaxPage.PageEngine.UpdateControlRender(this);
        }
    }
}