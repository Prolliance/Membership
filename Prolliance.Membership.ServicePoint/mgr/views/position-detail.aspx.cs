using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "position", Code = "view")]
    [SDK.Operation(TargetCode = "position", Code = "edit")]
    [SDK.Operation(TargetCode = "position", Code = "new")]
    public partial class PositionDetail : PageBase
    {

        private Dictionary<string, string> Args { get; set; }
        protected Position Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            if (!string.IsNullOrWhiteSpace(Args["id"]))
            {
                this.Model = Position.GetPositionById(Args["id"]);
            }
            else
            {
                this.Model = Position.Create();
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
            if (string.IsNullOrWhiteSpace(this.Model.Code))
            {
                this.PageEngine.ShowMessageBox("编码不合法");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Model.Name))
            {
                this.PageEngine.ShowMessageBox("名称不合法");
                return;
            }
            try
            {
                //编辑存在的组织
                if (!string.IsNullOrWhiteSpace(Args["id"]))
                {
                    this.Model.Save();
                }
                else
                {
                    this.Model.Save();
                    Organization org = Organization.GetOrganizationById(Args["orgId"]);
                    if (org != null)
                    {
                        org.AddPosition(this.Model);
                    }
                }

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