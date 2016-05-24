using AjaxEngine.Utils;
using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "role", Code = "*")]
    public partial class RoleList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Bind(null);
            }
        }

        protected void new_Click(object sender, EventArgs e)
        {
            this.PageEngine.OpenWindow<string, string>("role-detail.aspx", "role-detail", "width=600,height=278,resizeable=no", "", this.Bind);
        }

        [AjaxMethod]
        public void Bind(string str)
        {
            this.dataList.DataSource = Role.GetRoleList();
            this.dataList.DataBind();
            this.PageEngine.UpdateControlRender(this.tableArea);
            this.PageEngine.InvokeClientScript("initSort();");
        }

        [AjaxMethod]
        public void SaveSort(List<string> list)
        {
            var roleList = Role.GetRoleList().Where(role => list.Contains(role.Id));
            var sortIndex = list.Count();
            foreach (var id in list)
            {
                var role = roleList.FirstOrDefault(r => r.Id == id);
                role.Sort = sortIndex;
                role.Save();
                sortIndex--;
            }
            this.PageEngine.ShowMessageBox("排序已保存");
        }

        [AjaxMethod]
        public void Delete(string id)
        {
            Role model = Role.GetRoleById(id);
            if (model != null)
            {
                model.Delete();
                this.Bind(null);
            }
            else
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的角色", id));
            }
        }

        [AjaxMethod]
        public void Cancel(string id) { }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "detail":
                    {
                        this.PageEngine.OpenWindow<string, string>("role-detail.aspx", "role-detail", "width=600,height=278,resizeable=no", e.CommandArgument.ToString(), this.Bind);
                        break;
                    }
                case "delete":
                    {
                        this.PageEngine.ShowConfirmBox<string>("确认删除吗", this.Delete, this.Cancel, e.CommandArgument.ToString());
                        break;
                    }
                case "config":
                    {
                        this.PageEngine.OpenWindow<string, string>("role-operation.aspx", "role-operation", "width=600,height=520,resizeable=no", e.CommandArgument.ToString(), this.Bind);
                        break;
                    }
                case "mutex":
                    {
                        this.PageEngine.ShowMessageBox("暂不公开");
                        break;
                    }
            }
        }
    }
}