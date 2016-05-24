using Prolliance.Membership.Business;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace Prolliance.Membership.ServicePoint.Mgr.Views.Controls
{
    public partial class OrgTree : ControlBase
    {
        private List<Organization> OrgList = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                this.Bind();
            }
        }

        public void Bind()
        {
            this.OrgList = Organization.GetOrganizationList();
            TreeNode root = new TreeNode("组织机构树", "");
            this.RecordTreeState();
            root.Expanded = true;//根目录永远展开
            root.Selected = true;//默认选中
            this.CreateTree(root);
            this.tree.Nodes.Clear();
            this.tree.Nodes.Add(root);
        }

        protected void CreateTree(TreeNode parentNode)
        {
            if (this.OrgList != null)
            {
                List<Organization> orgList = this.OrgList.Where(org => org.ParentId == parentNode.Value).ToList();
                foreach (Organization org in orgList)
                {
                    TreeNode node = new TreeNode();
                    node.Text = org.Name;
                    node.Value = org.Id;
                    node.Expanded = TreeState_ExpandState.ContainsKey(node.Value) ? TreeState_ExpandState[node.Value] : false;
                    node.Selected = TreeState_SelectedValue == node.Value;
                    CreateTree(node);
                    parentNode.ChildNodes.Add(node);
                }
            }
        }

        #region 树展开及选中的状态
        private string TreeState_SelectedValue
        {
            set
            {
                Session["TreeState_SelectedValue"] = value;
            }
            get
            {
                return (string)Session["TreeState_SelectedValue"];
            }
        }
        private Dictionary<string, bool> TreeState_ExpandState
        {
            get
            {
                return (Dictionary<string, bool>)Session["TreeState_ExpandState"];
            }
            set
            {
                Session["TreeState_ExpandState"] = value;
            }
        }
        public void RecordTreeState()
        {
            if (this.TreeState_ExpandState == null)
            {
                this.TreeState_ExpandState = new Dictionary<string, bool>();
            }
            //this.TreeState_SelectedValue = null;
            if (this.tree.Nodes.Count > 0)
            {
                this.RecordTreeState(this.tree.Nodes[0]);
            }
        }
        private void RecordTreeState(TreeNode parentNode)
        {
            TreeState_ExpandState[parentNode.Value] = (parentNode.Expanded ?? false);
            TreeState_SelectedValue = parentNode.Selected ? parentNode.Value : TreeState_SelectedValue;
            foreach (TreeNode childNode in parentNode.ChildNodes)
            {
                RecordTreeState(childNode);
            }
        }
        #endregion

        public TreeNode SelectedNode
        {
            get
            {
                return this.tree.SelectedNode;
            }
        }

        public TreeView TreeView
        {
            get
            {
                return this.tree;
            }
        }

        protected void tree_SelectedNodeChanged(object sender, EventArgs e)
        {
            if (this.Page is PageBase)
            {
                this.RecordTreeState();
                this.CurrentPage.PageEngine.UpdateControlRender(this);
            }
        }

        protected void tree_TreeNodeCollapsed(object sender, TreeNodeEventArgs e)
        {
            this.RecordTreeState();
            this.CurrentPage.PageEngine.UpdateControlRender(this);
        }

        protected void tree_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            this.RecordTreeState();
            this.CurrentPage.PageEngine.UpdateControlRender(this);
        }
    }
}