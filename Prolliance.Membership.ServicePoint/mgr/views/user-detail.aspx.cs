using System.Linq;
using System.Web.UI;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "user", Code = "view")]
    [SDK.Operation(TargetCode = "user", Code = "edit")]
    [SDK.Operation(TargetCode = "user", Code = "new")]
    public partial class UserDetail : PageBase
    {
        private Dictionary<string, string> Args { get; set; }
        protected Business.User Model { get; set; }
        protected Organization CurrentOrganization { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            if (!string.IsNullOrWhiteSpace(Args["id"]))
            {
                this.Model = Business.User.GetUserById(Args["id"]);
                if (!IsPostBack)
                {
                    dataList.DataSource = this.Model.Extensions;
                    dataList.DataBind();
                    this.PageEngine.UpdateControlRender(this.tableArea);
                }
                this.ctl_Account.ReadOnly = true;
            }
            else
            {
                this.Model = Business.User.Create();
                if (!IsPostBack)
                {
                    dataList.DataSource = GetExtensionDic();
                    dataList.DataBind();
                    this.PageEngine.UpdateControlRender(this.tableArea);
                }
                this.Model.IsActive = true;
                this.ctl_Account.ReadOnly = false;
            }
            if (!string.IsNullOrWhiteSpace(Args["orgId"]))
            {
                this.CurrentOrganization = Organization.GetOrganizationById(Args["orgId"]);
            }
            if (!IsPostBack)
            {
                this.BindPositionList();
                this.FillForm(this.Model);
            }
        }

        private Dictionary<string,string> GetExtensionDic()
        {
          var dic=  Business.User.GetTableColumns().Where(p=>AppSettings.UserInfoBase.All(q => q != p.Key));
            var dicnew = new Dictionary<string, string>();
            foreach (var d in dic)
            {
                dicnew.Add(d.Key,string.Empty);
            }
            return dicnew;
        } 

        public void BindPositionList()
        {
            if (this.CurrentOrganization != null)
            {
                this.positionList.DataSource = this.CurrentOrganization.PositionList;
                this.positionList.DataTextField = "Name";
                this.positionList.DataValueField = "Id";
                this.positionList.DataBind();
                if (this.Model != null && this.Model.PositionList != null)
                {
                    var userPositionList = this.Model.PositionList;
                    foreach (ListItem item in this.positionList.Items)
                    {
                        item.Selected = userPositionList.Exists(p => p.Id == item.Value);
                    }
                }
            }
        }

       

        protected void save_Click(object sender, EventArgs e)
        {
            this.FillModel(this.Model);
            if (string.IsNullOrWhiteSpace(this.Model.Account))
            {
                this.PageEngine.ShowMessageBox("账号不合法");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Model.Name))
            {
                this.PageEngine.ShowMessageBox("名称不合法");
                return;
            }
            try
            {
                //扩展字段
                this.Model.Extensions = FillDictionary();
                //编辑存在的用户
                if (!string.IsNullOrWhiteSpace(Args["id"]))
                {
                   
                    this.Model.Save();
                }
                else
                {
       
                    this.Model.Save();
                    if (this.CurrentOrganization != null)
                    {
                        this.CurrentOrganization.AddUser(this.Model);
                    }
                }
                //保存到指定岗位 
                foreach (ListItem item in this.positionList.Items)
                {
                    Position position = Position.GetPositionById(item.Value);
                    if (position != null)
                    {
                        if (item.Selected)
                        {
                            position.AddUser(this.Model);
                        }
                        else
                        {
                            position.RemoveUser(this.Model);
                        }
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
               var keyTxt= control.FindControl("_key") as TextBox;
               var value= control.FindControl("_extensionVal") as TextBox;
               dic.Add(keyTxt.Text,value.Text);
            }

            return dic;
        } 
    }
}