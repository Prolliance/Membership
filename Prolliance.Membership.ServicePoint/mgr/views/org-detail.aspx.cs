using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "organization", Code = "view")]
    [SDK.Operation(TargetCode = "organization", Code = "edit")]
    [SDK.Operation(TargetCode = "organization", Code = "new")]
    public partial class OrgDetail : PageBase
    {

        private Dictionary<string, string> Args { get; set; }
        protected Organization Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            if (!string.IsNullOrWhiteSpace(Args["id"]))
            {
                this.Model = Organization.GetOrganizationById(Args["id"]);
                if (!IsPostBack)
                {
                    dataList.DataSource = this.Model.Extensions;
                    dataList.DataBind();
                    this.PageEngine.UpdateControlRender(this.tableArea);
                }
            }
            else
            {
                this.Model = Organization.Create();
                if (!IsPostBack)
                {
                    dataList.DataSource = GetExtensionDic();
                    dataList.DataBind();
                    this.PageEngine.UpdateControlRender(this.tableArea);
                }
                this.Model.IsActive = true;
            }
            if (!IsPostBack)
            {
                this.FillForm(this.Model);
            }
        }

        private Dictionary<string, string> GetExtensionDic()
        {
            var dic = Business.Organization.GetTableColumns().Where(p => AppSettings.OrgInfoBase.All(q => q != p.Key));
            var dicnew = new Dictionary<string, string>();
            foreach (var d in dic)
            {
                dicnew.Add(d.Key, string.Empty);
            }
            return dicnew;
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
                this.Model.Extensions = FillDictionary();
                //编辑存在的组织
                if (!string.IsNullOrWhiteSpace(Args["id"]))
                {
                    this.Model.Save();
                }
                else
                {
                    this.Model.Save();
                    Organization parent = Organization.GetOrganizationById(Args["parentId"]);
                    if (parent != null)
                    {
                        parent.AddChild(this.Model);
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

        private Dictionary<string, object> FillDictionary()
        {
            var dic = new Dictionary<string, object>();
            foreach (Control control in this.dataList.Items)
            {
                var keyTxt = control.FindControl("_key") as TextBox;
                var value = control.FindControl("_extensionVal") as TextBox;
                dic.Add(keyTxt.Text, value.Text);
            }

            return dic;
        } 
    }
}