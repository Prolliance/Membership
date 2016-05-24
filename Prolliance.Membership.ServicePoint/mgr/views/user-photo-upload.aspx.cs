using System.Linq;
using System.Web.UI;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.mgr.views
{
    [SDK.Operation(TargetCode = "user", Code = "view")]
    [SDK.Operation(TargetCode = "user", Code = "edit")]
    [SDK.Operation(TargetCode = "user", Code = "new")]
    public partial class UserPhotoUpload : PageBase
    {
        private Dictionary<string, string> Args { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            if(!IsPostBack)
            {
                this.PageEngine.RemoveAjaxAble(save);

                var url = this.Request.Url.PathAndQuery.Substring(0, this.Request.Url.PathAndQuery.IndexOf(@"/mgr/"));

                imgPhoto.ImageUrl = string.Format("{0}/service/user.ashx?method=GetUserPhotoByAccount&account={1}", url, Args["Account"]);
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {
            if(!filPhoto.HasFile)
            {
                this.PageEngine.ShowMessageBox("请选择头像文件");
                return;
            }

            List<string> ext = new List<string>() { ".JPG", ".PNG", ".GIF" };
            string filExt = System.IO.Path.GetExtension(filPhoto.FileName).ToUpper();
            if (ext.IndexOf(filExt) == -1)
            {
                this.PageEngine.ShowMessageBox("文件格式不合法");
                return;
            }

            var userPhoto = UserPhoto.GetUserPhotoByAccount(Args["Account"]);
            if (userPhoto == null)
            {
                userPhoto = UserPhoto.Create();
                userPhoto.Account = Args["Account"];
            }

            userPhoto.PhotoBinary = filPhoto.FileBytes;
            userPhoto.PhotoExt = filExt;
            userPhoto.Save();

            this.PageEngine.ShowMessageBox("上传成功");
            this.PageEngine.CloseWindow();
        }
    }
}