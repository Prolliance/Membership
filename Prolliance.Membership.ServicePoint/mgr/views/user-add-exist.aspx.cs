using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "user", Code = "add")]
    public partial class UserAddExtist : PageBase
    {
        private Dictionary<string, string> Args { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void find_Click(object sender, EventArgs e)
        {
            var keyword = ctl_keyword.Text.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                List<User> foundUserList = Business.User.GetUserList()
                    .Where(user => user.Account.ToLower().Contains(keyword)
                           || user.Name.ToLower().Contains(keyword)
                           || user.Email.ToLower().Contains(keyword))
                           .Take(7)
                           .ToList();
                if (foundUserList == null || foundUserList.Count < 1)
                {
                    this.PageEngine.ShowMessageBox("没有找到匹配用户");
                }
                this.dataList.DataSource = foundUserList;
                this.dataList.DataBind();
                this.PageEngine.UpdateControlRender(this.tableArea);
            }
            else
            {
                this.PageEngine.ShowMessageBox("请输入关键词");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void save_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Args["orgId"]))
            {
                try
                {
                    Organization org = Organization.GetOrganizationById(Args["orgId"]);
                    foreach (RepeaterItem item in this.dataList.Items)
                    {
                        CheckBox check = (CheckBox)item.FindControl("userId");
                        if (check != null && check.Checked == true)
                        {
                            var userId = check.Attributes["data-id"];
                            var user = Business.User.GetUserById(userId);
                            org.AddUser(user);
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
        }

        protected void loadRemote_Click(object sender, EventArgs e)
        {
            try
            {
                //Thread.Sleep(3000);
                Business.User.LoadUserRemoteUser();
                this.PageEngine.ShowMessageBox("完成");
                this.PageEngine.UpdateControlRender(this.loadRemote);
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }
    }
}