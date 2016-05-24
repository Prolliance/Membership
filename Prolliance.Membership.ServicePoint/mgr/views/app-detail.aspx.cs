using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "app", Code = "new")]
    [SDK.Operation(TargetCode = "app", Code = "view")]
    [SDK.Operation(TargetCode = "app", Code = "edit")]
    public partial class AppDetail : PageBase
    {
        protected App Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.PageEngine.GetWindowArgs<string>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                this.Model = App.GetAppById(id);
            }
            else
            {
                this.Model = App.Create();
                this.Model.IsActive = true;
            }
            if (!IsPostBack)
            {
                this.FillForm(this.Model);
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {

            this.FillModel(this.Model);
            if (string.IsNullOrWhiteSpace(this.Model.Name))
            {
                this.PageEngine.ShowMessageBox("名称不合法");
                return;
            }
            try
            {
                this.Model.Save();
                this.PageEngine.ReturnValue("");
                this.PageEngine.CloseWindow();
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }

        protected void resetSecret_Click(object sender, EventArgs e)
        {
            this.ctl_Secret.Text = StringFactory.NewGuid();
            this.PageEngine.UpdateControlRender(this.ctl_Secret);
        }
    }
}