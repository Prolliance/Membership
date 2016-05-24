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
    [SDK.Operation(TargetCode = "app", Code = "*")]
    public partial class AppList : PageBase
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
            this.PageEngine.OpenWindow<string, string>("app-detail.aspx", "app-detail", "width=600,height=380,resizeable=no", "", this.Bind);
        }
        [AjaxMethod]
        public void Bind(string str)
        {
            this.dataList.DataSource = App.GetAppList();
            this.dataList.DataBind();
            this.PageEngine.UpdateControlRender(this.tableArea);
            this.PageEngine.InvokeClientScript("initSort();");
        }
        [AjaxMethod]
        public void SaveSort(List<string> list)
        {
            var itemList = App.GetAppList().Where(x => list.Contains(x.Id));
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
            App app = App.GetAppById(id);
            if (app != null)
            {
                app.Delete();
                this.Bind(null);
            }
            else
            {
                this.PageEngine.ShowMessageBox(string.Format("没有找到 id 为 ‘{0}’ 的应用", id));
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
                        this.PageEngine.OpenWindow<string, string>("app-detail.aspx", "app-detail", "width=600,height=380,resizeable=no", e.CommandArgument.ToString(), this.Bind);
                        break;
                    }
                case "delete":
                    {
                        this.PageEngine.ShowConfirmBox<string>("确认删除吗", this.Delete, this.Cancel, e.CommandArgument.ToString());
                        break;
                    }
                case "manifest":
                    {
                        this.PageEngine.OpenWindow<string, string>("app-manifest.aspx", "app-manifest", "width=600,height=520,resizeable=no", e.CommandArgument.ToString(), this.Bind);
                        break;
                    }
            }
        }
    }
}