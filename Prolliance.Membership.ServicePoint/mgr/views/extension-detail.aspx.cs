
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Views
{

    [SDK.Operation(TargetCode = "ext", Code = "new")]
    public partial class ExtensionDetail : PageBase
    {

        private Dictionary<string, string> Args { get; set; }
        protected ExtensionField Model { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Args = this.PageEngine.GetWindowArgs<Dictionary<string, string>>();
            //if (!string.IsNullOrWhiteSpace(Args["id"]))
            //{
                
            //}
            //else
            //{
               
              
            //}
            this.Model = new ExtensionField();
            if (!string.IsNullOrWhiteSpace(Args["type"]))
            {
                this.Model.TableName = Args["type"];
            }
            
          
            if (!IsPostBack)
            {
               
                this.FillForm(this.Model);
            }
        }

        protected void save_Click(object sender, EventArgs e)
        {
            this.Model.ColumnName = this.ctl_ColumnName.Text.Trim();
            this.Model.Defaultval = this.ctl_Defaultval.Text.Trim();
            var type = ColumnType.Number;
            ColumnType.TryParse(this.ctl_Type.SelectedValue, out type);
            this.Model.Type = type;
            this.Model.Des = this.ctl_Des.Text.Trim();
            this.Model.Length = int.Parse(this.ctl_Length.Text.Trim());
           
            //this.FillModel(this.Model);
            if (string.IsNullOrWhiteSpace(this.Model.ColumnName))
            {
                this.PageEngine.ShowMessageBox("字段名称不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.Model.Des))
            {
                this.PageEngine.ShowMessageBox("字段描述不能为空");
                return;
            }
            try
            {
                
                //编辑存在的组织
                if (Args["type"] == "")
                {
                    this.PageEngine.ShowMessageBox("请选择表");
                    return;
                }
                    Organization.AddExtensionField(this.Model);
                this.PageEngine.ReturnValue("");
                this.PageEngine.CloseWindow();
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }

        protected void ctl_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ctl_Type.SelectedValue == "1")
            {
                this.litLen.Text = "小数位数";
            }
        }
    }
}