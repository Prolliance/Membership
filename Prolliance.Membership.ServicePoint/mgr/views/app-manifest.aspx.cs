using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{
    [SDK.Operation(TargetCode = "app", Code = "manifest")]
    public partial class AppManifest : PageBase
    {
        protected App Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = this.PageEngine.GetWindowArgs<string>();
            this.Model = App.GetAppById(id);
            if (!IsPostBack && this.Model != null)
            {
                this.manifestBox.Text = this.Model.ExportManifestText();
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {
            #region 输入验理证
            if (string.IsNullOrWhiteSpace(this.manifestBox.Text))
            {
                this.manifestBox.Text = "[]";
            }
            #endregion

            try
            {
                this.Model.ImportManifestText(this.manifestBox.Text);
                this.PageEngine.CloseWindow();
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }
    }
}