<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="position-list.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.PositionList" %>

<%@ Register Src="~/mgr/views/Controls/OrgTree.ascx" TagPrefix="uc1" TagName="OrgTree" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>职位列表</title>
    <link href="../static/view.css" rel="stylesheet" />
    <link href="../../static/jqueryui/css/jquery-ui.css" rel="stylesheet" />
    <style>
        #tree-area { position: fixed; top: 0px; left: 0px; border-right: solid 1px #ddd; height: 100%; overflow: auto; width: 250px; }
        #content-area { padding-left: 250px; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server">
        <div id="tree-area">
            <div>
                <uc1:OrgTree runat="server" ID="OrgTree" />
            </div>
        </div>
        <div id="content-area">
            <div class="content">
                <div class="view">
                    <div class="toolbar">
                        <asp:Button ID="new" runat="server" data-auth="[{TargetCode:'position',Code:'new'}]" Text="新建职位" OnClick="new_Click" />
                    </div>
                    <div id="tableArea" runat="server">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th>名称</th>
                                    <th style="width: 45px;">状态</th>
                                    <th style="width: 150px;" id="opArea" runat="server" data-auth="[{TargetCode:'position',Code:'role'},{TargetCode:'position',Code:'report'},{TargetCode:'position',Code:'edit'},{TargetCode:'position',Code:'view'},{TargetCode:'position',Code:'delete'}]">操作</th>
                                </tr>
                            </thead>
                            <tbody class="sortable-list">
                                <asp:Repeater ID="dataList" runat="server" OnItemCommand="dataList_ItemCommand">
                                    <ItemTemplate>
                                        <tr data-id="<%# Eval("Id") %>">
                                            <td><%# Eval("Name") %></td>
                                            <td><%# Convert.ToBoolean(Eval("IsActive"))?"启用":"禁用" %></td>
                                            <td id="opArea" runat="server" data-auth="[{TargetCode:'position',Code:'role'},{TargetCode:'position',Code:'report'},{TargetCode:'position',Code:'edit'},{TargetCode:'position',Code:'view'},{TargetCode:'position',Code:'delete'}]">
                                                <asp:LinkButton ID="role" CommandName="role" CommandArgument='<%# Eval("Id") %>' data-auth="[{TargetCode:'position',Code:'role'}]" runat="server">角色</asp:LinkButton>
                                                <asp:LinkButton ID="report" CommandName="report" CommandArgument='<%# Eval("Id") %>' data-auth="[{TargetCode:'position',Code:'report'}]" runat="server">汇报</asp:LinkButton>
                                                <asp:LinkButton ID="detail" CommandName="detail" CommandArgument='<%# Eval("Id") %>' data-auth="[{TargetCode:'position',Code:'edit'},{TargetCode:'position',Code:'view'}]" runat="server">详细</asp:LinkButton>
                                                <asp:LinkButton ID="delete" CommandName="delete" CommandArgument='<%# Eval("Id") %>' data-auth="[{TargetCode:'position',Code:'delete'}]" runat="server">删除</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <script src="../../static/jqueryui/js/jqueryui.js"></script>
        <script src="../static/table.js"></script>
    </form>
</body>
</html>
 