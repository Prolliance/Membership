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
    [SDK.Operation(TargetCode = "ext", Code = "*")]
    public partial class extensioninfo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //this.Bind(null);
            }
        }


        [AjaxMethod]
        public void Bind(string str)
        {
            switch (dropSelectTB.SelectedValue)
            {
                case "User":
                    this.dataList.DataSource = Business.User.GetTableColumns();
                    this.dataList.DataBind();
                    break;
                case "Organization":
                    this.dataList.DataSource = Business.Organization.GetTableColumns();
                    this.dataList.DataBind();
                    break;

            }
            this.PageEngine.UpdateControlRender(this.tableArea);
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


     

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            var args = new
            {
                type = dropSelectTB.SelectedValue,
            };
            this.PageEngine.OpenWindow<object, string>("extension-detail.aspx", "extension-detail", "width=600,height=380,resizeable=no", args, this.Bind);
     
        }

        protected void btnSubmit_Click1(object sender, EventArgs e)
        {
            if (dropSelectTB.SelectedValue != string.Empty)
            {
                switch (dropSelectTB.SelectedValue)
                {
                    case "User":
                        this.dataList.DataSource = Business.User.GetTableColumns();
                        this.dataList.DataBind();
                        break;
                    case "Organization":
                        this.dataList.DataSource = Business.Organization.GetTableColumns();
                        this.dataList.DataBind();
                        break;

                }
                this.PageEngine.UpdateControlRender(this.tableArea);

            }
        }
      
    }
}