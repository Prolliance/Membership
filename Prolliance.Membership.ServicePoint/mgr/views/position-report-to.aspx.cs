using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.mgr.views
{
    [SDK.Operation(TargetCode = "position", Code = "*")]
    public partial class PositionReportTo : PageBase
    {
        private string Id { get; set; }

        protected Position Model { get; set; }

        protected List<string> SelectedList
        {
            get
            {
                if (ViewState["SelectedList"] == null)
                {
                    ViewState["SelectedList"] = new List<string>();
                }
                return (List<string>)ViewState["SelectedList"];
            }
            set
            {
                ViewState["SelectedList"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.OrgTree.TreeView.SelectedNodeChanged += TreeView_SelectedNodeChanged;
            this.Id = this.PageEngine.GetWindowArgs<string>();
            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                this.Model = Position.GetPositionById(this.Id);
            }
            if (!IsPostBack)
            {
                if (this.Model != null)
                {
                    this.Model = Position.GetPositionById(this.Id);
                    if (this.Model != null && this.Model.ReportToList != null)
                    {
                        var list = this.Model.ReportToList;
                        this.SelectedList = list.Select(p => p.Id).ToList();
                    }
                }
                this.BindSelected();
                this.BindList();
            }
        }

        void TreeView_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.BindList();
        }

        public void BindSelected()
        {
            var selectedList = Position.GetPositionList()
                .Where(p => this.SelectedList.Contains(p.Id))
                .ToList();
            var fullNameList = new List<string>();
            foreach (Position postion in selectedList)
            {
                if (postion == null) continue;
                List<string> buffer = new List<string>();
                buffer.Insert(0, postion.Name);
                var org = postion.GetOrganization();
                if (org != null)
                {
                    buffer.Insert(0, org.Name);
                    var parentOrgList = org.DeepParentList;
                    if (parentOrgList != null)
                    {
                        buffer.InsertRange(0, parentOrgList.Select(o2 => o2.Name));
                    }
                }
                fullNameList.Add(string.Join("/", buffer.ToArray()));
            }
            this.selectedDataList.DataSource = fullNameList;
            this.selectedDataList.DataBind();
            this.PageEngine.UpdateControlRender(this.selectedDataListArea);
        }

        public void BindList()
        {
            var orgId = this.OrgTree.TreeView.SelectedValue ?? "";
            this.dataList.DataSource = Position.GetPositionList()
                       .Where(org => org.OrganizationId == orgId);
            this.dataList.DataBind();
            this.PageEngine.UpdateControlRender(this.tableArea);
        }

        protected void save_Click(object sender, EventArgs e)
        {
            try
            {
                var willRemoveList = this.Model.ReportToList;
                foreach (Position position in willRemoveList)
                {
                    this.Model.RemoveReportTo(position);
                }
                var willAddList = Position.GetPositionList()
                                           .Where(p => this.SelectedList.Contains(p.Id))
                                           .ToList();
                foreach (Position position in willAddList)
                {
                    this.Model.AddReportTo(position);
                }
                this.PageEngine.ReturnValueAndCloseWindow("");
            }
            catch (Exception ex)
            {
                this.PageEngine.ShowMessageBox(ex.Message);
            }
        }

        protected void dataList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            var id = Convert.ToString(e.CommandArgument);
            var action = e.CommandName;
            if (!string.IsNullOrWhiteSpace(id))
            {

                if (action == "select")
                {
                    this.SelectedList.Add(id);
                }
                else
                {
                    this.SelectedList.Remove(id);
                }
                this.SelectedList = this.SelectedList;
                this.BindSelected();
                this.BindList();
            }
        }
    }
}