using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "role", Code = "view")]
    [SDK.Operation(TargetCode = "role", Code = "edit")]
    [SDK.Operation(TargetCode = "role", Code = "new")]
    public partial class RoleDetail : PageBase
    {

        protected Role Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.PageEngine.GetWindowArgs<string>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                this.Model = Role.GetRoleById(id);
            }
            else
            {
                this.Model = Role.Create();
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
            if (string.IsNullOrWhiteSpace(this.Model.Code))
            {
                this.PageEngine.ShowMessageBox("编码不合法");
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
    }
}