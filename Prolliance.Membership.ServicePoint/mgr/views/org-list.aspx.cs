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
    [SDK.Operation(TargetCode = "organization", Code = "*")]
    public partial class OrgList : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            this.OrgTree.TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            if (!IsPostBack)
            {
                this.Bind(null);
            }
        }

        void TreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.Bind(null);
        }

        protected void new_Click(object sender, EventArgs e)
        {
            var args = new
            {
                parentId = this.OrgTree.TreeView.SelectedValue,
                id = ""
            };
            this.PageEngine.OpenWindow<object, string>("org-detail.aspx", "org-detail", "width=600,height=285,resizeable=no", args, this.BindAll);
        }
        [AjaxMethod]
        public void Bind(string str)
        {
            var parentId = this.OrgTree.TreeView.SelectedValue ?? "";
            this.dataList.DataSource = Organization.GetOrganizationList()
                       .Where(org => org.ParentId == parentId);
            this.dataList.DataBind();
            this.PageEngine.UpdateControlRender(this.tableArea);
            this.PageEngine.InvokeClientScript("initSort();");
        }
        [AjaxMethod]
        public void SaveSort(List<string> list)
        {
            var itemList = Organization.GetOrganizationList().Where(x => list.Contains(x.Id));
            var sortIndex = list.Count();
            foreach (var id in list)
            {
                var item = itemList.FirstOrDefault(x => x.Id == id);
                item.Sort = sortIndex;
                item.Save();
                sortIndex--;
            }
            this.PageEngine.ShowMessageBox("排序已保存");
            this.OrgTree.Bind();
            this.PageEngine.UpdateControlRender(this.OrgTree);
        }
        [AjaxMethod]
        public void BindAll(string str)
        {
            this.OrgTree.Bind();
            this.PageEngine.UpdateControlRender(this.OrgTree);
            this.Bind(null);
        }

        [AjaxMethod]
        public void Delete(string id)
        {
            Organization org = Organization.GetOrganizationById(id);
            if (org != null)
            {
                org.Delete();
                this.BindAll(null);
            }
            else
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的应用", id));
            }
        }
        [AjaxMethod]
        public void Cancel(string str) { }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "detail":
                    {
                        var args = new
                        {
                            parentId = this.OrgTree.TreeView.SelectedValue,
                            id = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("org-detail.aspx", "org-detail", "width=600,height=285,resizeable=no", args, this.BindAll);
                        break;
                    }
                case "delete":
                    {
                        this.PageEngine.ShowConfirmBox<string>("确认删除吗", this.Delete, this.Cancel, e.CommandArgument.ToString());
                        break;
                    }
                case "role":
                    {
                        var args = new
                        {
                            type = "organization",
                            id = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("role-give.aspx", "org-give", "width=600,height=500,resizeable=no", args, this.Bind);
                        break;
                    }
            }
        }
    }
}