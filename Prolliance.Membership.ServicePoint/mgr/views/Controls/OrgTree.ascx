<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgTree.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.Controls.OrgTree" %>
<div id="<%=this.ClientID %>" class="tree">
    <asp:TreeView ID="tree" runat="server" SkipLinkText="" OnSelectedNodeChanged="tree_SelectedNodeChanged" EnableClientScript="False" ExpandDepth="1" NodeIndent="15" OnTreeNodeCollapsed="tree_TreeNodeCollapsed" OnTreeNodeExpanded="tree_TreeNodeExpanded" PopulateNodesFromClient="False" ShowLines="True" Width="100%">
        <NodeStyle CssClass="node" />
        <SelectedNodeStyle CssClass="selected" />
    </asp:TreeView>
</div>
