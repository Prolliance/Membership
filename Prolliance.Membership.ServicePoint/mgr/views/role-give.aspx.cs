using AjaxEngine.Extends;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using Prolliance.Membership.ServicePoint.Lib.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "user", Code = "role")]
    [SDK.Operation(TargetCode = "position", Code = "role")]
    [SDK.Operation(TargetCode = "organization", Code = "role")]
    public partial class RoleGive : PageBase
    {
        protected Dictionary<string, string> Args { get; set; }
        protected Object Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            this.Model = this.GetModel();

            if (!IsPostBack)
            {
                this.Bind(null);
            }
        }

        public object GetModel()
        {
            string type = this.Args["type"];
            string id = this.Args["id"];
            switch (type)
            {
                case "organization":
                    {
                        return Organization.GetOrganizationById(id);
                    }
                case "user":
                    {
                        return Business.User.GetUserById(id);
                    }
                case "position":
                    {
                        return Position.GetPositionById(id);
                    }
            }
            return null;
        }

        protected void Bind(string str)
        {
            List<Role> roleList = Role.GetRoleList();
            //如果当前用户没有 “配置 Control Panle 的权限” 的权限。
            //则排除有 “配置 Control Panle 的权限” 的角色，防止绕过权限。
            if (!this.CurrentUser
                    .CheckPermissionBySDK(this.AuthManifest.Role.SystemAuthConfig))
            {
                roleList = roleList
                    .Where(role => !role
                        .OperationList
                        .Exists(op => op.AppKey == AppSettings.Name
                            && op.TargetCode == this.AuthManifest.Role.SystemAuthConfig.TargetCode
                            && op.Code == this.AuthManifest.Role.SystemAuthConfig.Code))
                    .ToList();
            }
            //--
            this.dataList.DataSource = roleList;
            this.dataList.DataBind();
        }

        private List<Role> SelectedRoleList { get; set; }
        public bool Check(Role role)
        {
            if (this.Model == null) return false;
            if (SelectedRoleList == null)
            {
                this.SelectedRoleList = (List<Role>)this.Model.GetPropertyValue("RoleList");
            }
            return this.SelectedRoleList.Exists(r => r.Id == role.Id);
        }

        protected void save_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.dataList.Items)
            {
                CheckBox check = (CheckBox)item.FindControl("roleId");
                var roleId = check.Attributes["data-id"];
                var role = Role.GetRoleById(roleId);
                if (check != null && check.Checked == true)
                {
                    this.Model.InvokeMethod("GiveRole", new object[] { role });
                }
                else
                {
                    this.Model.InvokeMethod("CancelRole", new object[] { role });
                }
            }
            this.PageEngine.CloseWindow();
        }
    }
}