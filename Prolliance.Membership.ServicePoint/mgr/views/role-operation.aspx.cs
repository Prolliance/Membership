using AjaxEngine.Utils;
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
    [SDK.Operation(TargetCode = "role", Code = "auth-config")]
    [SDK.Operation(TargetCode = "role", Code = "sys-auth-config")]
    public partial class RoleOperation : PageBase
    {
        protected Role Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Model = this.GetModel();
            if (!IsPostBack)
            {
                this.Bind();
            }
        }
        public Role GetModel()
        {
            var id = this.PageEngine.GetWindowArgs<string>();
            if (!string.IsNullOrWhiteSpace("id"))
            {
                return Role.GetRoleById(id);
            }
            return null;
        }
        protected void Bind()
        {
            List<App> appList = App.GetAppList();
            if (appList != null)
            {
                //排除没有声明权限的应用
                appList = appList
                    .Where(app => app.TargetList != null
                        && app.TargetList.Count > 0)
                    .ToList();
                //检查是否有配置 Control Panle 的权限，防止绕过权限。
                if (!this.CurrentUser
                    .CheckPermissionBySDK(this.AuthManifest.Role.SystemAuthConfig))
                {
                    appList = appList.Where(a => a.Key != AppSettings.Name).ToList();
                }
                //
                this.appList.DataSource = appList;
                this.appList.DataBind();
            }
        }
        [AjaxMethod]
        public void Save(List<string> giveIdList, List<string> cancelIdList)
        {
            this.Model = this.GetModel();
            try
            {
                if (this.Model != null && giveIdList != null)
                {
                    //处理取消的操作
                    foreach (var id in cancelIdList)
                    {
                        Operation operation = Operation.GetOperationById(id);
                        if (operation != null)
                        {
                            this.Model.CancelPermission(operation);
                        }
                    }
                }
                if (this.Model != null && cancelIdList != null)
                {
                    //处理取消的操作
                    foreach (var id in giveIdList)
                    {
                        Operation operation = Operation.GetOperationById(id);
                        if (operation != null)
                        {
                            this.Model.GivePermission(operation);
                        }
                    }
                }
                this.PageEngine.CloseWindow();
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }

        private List<Operation> currentRoleOperationList { get; set; }
        public bool Check(Operation operation)
        {
            if (currentRoleOperationList == null)
            {
                currentRoleOperationList = this.Model.OperationList;
            }
            if (currentRoleOperationList != null)
            {
                return currentRoleOperationList.Exists(op => op.Id == operation.Id);
            }
            return false;
        }

        protected void appList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            App app = (App)e.Item.DataItem;
            List<Target> targetList = app.TargetList;
            Dictionary<string, List<Target>> groupTargetList = new Dictionary<string, List<Target>>();
            foreach (Target target in targetList)
            {
                if (!groupTargetList.ContainsKey(target.Group))
                {
                    groupTargetList[target.Group] = new List<Target>();
                }
                groupTargetList[target.Group].Add(target);
            }
            Repeater groupList = (Repeater)e.Item.FindControl("groupList");
            groupList.ItemDataBound += groupList_ItemDataBound;
            groupList.DataSource = groupTargetList;
            groupList.DataBind();
        }

        void groupList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            KeyValuePair<string, List<Target>> groupTargetList = (KeyValuePair<string, List<Target>>)e.Item.DataItem;
            Repeater targetList = (Repeater)e.Item.FindControl("targetList");
            targetList.ItemDataBound += targetList_ItemDataBound;
            targetList.DataSource = groupTargetList.Value;
            targetList.DataBind();
        }

        void targetList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Target target = (Target)e.Item.DataItem;
            Repeater operationList = (Repeater)e.Item.FindControl("operationList");
            operationList.ItemDataBound += operationList_ItemDataBound;
            operationList.DataSource = target.OperationList;
            operationList.DataBind();
        }

        void operationList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
    }
}