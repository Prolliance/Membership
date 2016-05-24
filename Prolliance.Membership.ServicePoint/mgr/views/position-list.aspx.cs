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
    [SDK.Operation(TargetCode = "position", Code = "*")]
    public partial class PositionList : PageBase
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
                orgId = this.OrgTree.TreeView.SelectedValue,
                id = ""
            };
            this.PageEngine.OpenWindow<object, string>("position-detail.aspx", "position-detail", "width=600,height=258,resizeable=no", args, this.Bind);
        }
        [AjaxMethod]
        public void Bind(string str)
        {
            var orgId = this.OrgTree.TreeView.SelectedValue ?? "";
            this.@new.Enabled = !string.IsNullOrWhiteSpace(orgId);
            this.dataList.DataSource = Position.GetPositionList()
                       .Where(org => org.OrganizationId == orgId);
            this.dataList.DataBind();
            this.PageEngine.UpdateControlRender(this.tableArea);
            this.PageEngine.UpdateControlRender(this.@new);
            this.PageEngine.InvokeClientScript("initSort();");
        }

        [AjaxMethod]
        public void SaveSort(List<string> list)
        {
            var itemList = Position.GetPositionList().Where(x => list.Contains(x.Id));
            var sortIndex = list.Count();
            foreach (var id in list)
            {
                var item = itemList.FirstOrDefault(x => x.Id == id);
                item.Sort = sortIndex;
                item.Save();
                sortIndex--;
            }
            this.PageEngine.ShowMessageBox("排序已保存");
        }
        [AjaxMethod]
        public void Delete(string id)
        {
            Position position = Position.GetPositionById(id);
            if (position != null)
            {
                position.Delete();
                this.Bind(null);
            }
            else
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的应用", id));
            }
        }
        [AjaxMethod]
        public void Cancel(string key) { }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "detail":
                    {
                        var args = new
                        {
                            orgId = this.OrgTree.TreeView.SelectedValue,
                            id = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("position-detail.aspx", "position-detail", "width=600,height=258,resizeable=no", args, this.Bind);
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
                            type = "position",
                            id = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("role-give.aspx", "org-give", "width=600,height=500,resizeable=no", args, this.Bind);
                        break;
                    }
                case "report":
                    {
                        var id = e.CommandArgument.ToString();
                        this.PageEngine.OpenWindow<object, string>("position-report-to.aspx", "position-report-to", "width=600,height=500,resizeable=no", id, this.Cancel);
                        break;
                    }
            }
        }
    }
}