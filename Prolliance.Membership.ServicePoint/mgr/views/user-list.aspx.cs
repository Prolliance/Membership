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
    [SDK.Operation(TargetCode = "user", Code = "*")]
    public partial class UserList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.OrgTree.TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            if (!IsPostBack)
            {
                this.Bind(null);
                this.Pager.RefreshPager();
            }
        }

        void TreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.Bind(null);
            this.Pager.RefreshPager();
        }

        protected void new_Click(object sender, EventArgs e)
        {
            var args = new
            {
                orgId = this.OrgTree.TreeView.SelectedValue,
                id = ""
            };
            this.PageEngine.OpenWindow<object, string>("user-detail.aspx", "user-detail", "width=600,height=310,resizeable=no", args, this.Bind);
        }
        [AjaxMethod]
        public void Bind(string str)
        {
            var orgId = this.OrgTree.TreeView.SelectedValue ?? "";
            this.@new.Enabled = !string.IsNullOrWhiteSpace(orgId);
            this.add.Enabled = !string.IsNullOrWhiteSpace(orgId);
            List<User> userList = null;
            if (!string.IsNullOrWhiteSpace(orgId))
            {
                Organization org = Organization.GetOrganizationById(orgId);
                 userList = org.UserList;
                if (!string.IsNullOrEmpty(this.txtSearch.Text))
                {
                    userList =
                        userList.Where(
                            p => p.Account.Contains(this.txtSearch.Text) || p.Name.Contains(this.txtSearch.Text))
                            .ToList();
                }
                if (org != null)
                {
                    var total = userList.Count;
                    this.Pager.Visible = (total > 0);
                    this.Pager.TotalCount = total;
                    userList = userList.Skip(this.Pager.PageIndex*this.Pager.PageSize).Take(this.Pager.PageSize).ToList();
                }
            }
            else
            {
                this.Pager.Visible = false;
                return;
            }
            this.dataList.DataSource = userList;
            this.dataList.DataBind();

          
            this.PageEngine.UpdateControlRender(this.tableArea);
            this.PageEngine.UpdateControlRender(this.@new);
            this.PageEngine.UpdateControlRender(this.add);
            this.PageEngine.InvokeClientScript("initSort();");
        }
        [AjaxMethod]
        public void SaveSort(List<string> list)
        {
            var orgId = this.OrgTree.TreeView.SelectedValue ?? "";
            Organization org = Organization.GetOrganizationById(orgId);
            if (org != null)
            {
                org.SortUser(list);
                this.PageEngine.ShowMessageBox("排序已保存");
            }
        }
        [AjaxMethod]
        public void Delete(string id)
        {
            Business.User user = Business.User.GetUserById(id);
            if (user != null)
            {
                user.Delete();
                this.Bind(null);
            }
            else
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的用户", id));
            }
        }
        [AjaxMethod]
        public void Remove(string id)
        {
            var orgId = this.OrgTree.TreeView.SelectedValue ?? "";
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null)
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的组织", orgId));
                return;
            }
            Business.User user = Business.User.GetUserById(id);
            if (user == null)
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 Id 为 ‘{0}’ 的用户", id));

            }
            org.RemoveUser(user);
            this.Bind(null);
        }
        [AjaxMethod]
        public void Cancel(string str) { }

        public string GetPositionNameList(Business.User user)
        {
            if (user != null
                && user is Business.User
                && user.PositionList != null
                && user.PositionList.Count > 0)
            {
                List<Position> positionList = user.PositionList
                    .Where(p => p.OrganizationId == this.OrgTree.TreeView.SelectedValue)
                    .ToList();
                List<string> nameList = new List<string>();
                foreach (Position p in positionList)
                {
                    nameList.Add(p.Name);
                }
                return string.Join(" , ", nameList.ToArray());
            }
            else
            {
                return "";
            }
        }

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
                        this.PageEngine.OpenWindow<object, string>("user-detail.aspx", "user-detail", "width=600,height=310,resizeable=no", args, this.Bind);
                        break;
                    }
                case "photo":
                    {
                        var args = new
                        {
                            Account = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("user-photo-upload.aspx", "user-photo-upload", "width=350,height=285,resizeable=no", args, this.Bind);
                        break;
                    }
                case "password":
                    {
                        var userId = e.CommandArgument.ToString();
                        this.PageEngine.OpenWindow<object, string>("user-password.aspx", "user-password", "width=600,height=225,resizeable=no", userId, this.Bind);
                        break;
                    }
                case "delete":
                    {
                        this.PageEngine.ShowConfirmBox<string>("确认删除吗", this.Delete, this.Cancel, e.CommandArgument.ToString());
                        break;
                    }
                case "remove":
                    {
                        this.PageEngine.ShowConfirmBox<string>("确认从组织中移除吗", this.Remove, this.Cancel, e.CommandArgument.ToString());
                        break;
                    }
                case "role":
                    {
                        var args = new
                        {
                            type = "user",
                            id = e.CommandArgument.ToString()
                        };
                        this.PageEngine.OpenWindow<object, string>("role-give.aspx", "org-edit", "width=600,height=500,resizeable=no", args, this.Bind);
                        break;
                    }
            }
        }

        protected void add_Click(object sender, EventArgs e)
        {
            var args = new
            {
                orgId = this.OrgTree.TreeView.SelectedValue,
                id = ""
            };
            this.PageEngine.OpenWindow<object, string>("user-add-exist.aspx", "user-add-exist", "width=600,height=360,resizeable=no", args, this.Bind);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            this.Bind(null);
            this.Pager.RefreshPager();
        }

        protected void Pager1_Paging()
        {
            Bind(null);
        }
    }
}