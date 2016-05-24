<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role-list.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.RoleList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应用列表</title>
    <link href="../static/view.css" rel="stylesheet" />
    <link href="../../static/jqueryui/css/jquery-ui.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" class="content" runat="server">
        <div class="view">
            <div class="toolbar">
                <asp:Button ID="new" runat="server" Text="新建角色" data-auth="[{TargetCode:'role',Code:'new'}]" OnClick="new_Click" />
            </div>
            <div id="tableArea" runat="server">
                <table class="table">
                    <thead>
                        <tr>
                            <th style="width: 200px;">名称</th>
                            <th>描述</th>
                            <th style="width: 45px;">状态</th>
                            <th style="width: 150px;" id="opArea" runat="server" data-auth="[{TargetCode:'role',Code:'auth-config'},{TargetCode:'role',Code:'sys-auth-config'},{TargetCode:'role',Code:'mutex'},{TargetCode:'role',Code:'view'},{TargetCode:'role',Code:'edit'},{TargetCode:'role',Code:'delete'}]">操作</th>
                        </tr>
                    </thead>
                    <tbody class="sortable-list">
                        <asp:Repeater ID="dataList" runat="server" OnItemCommand="dataList_ItemCommand">
                            <ItemTemplate>
                                <tr data-id="<%# Eval("Id") %>">
                                    <td><%# Eval("Name") %></td>
                                    <td><%# Eval("Summary") %></td>
                                    <td><%# Convert.ToBoolean(Eval("IsActive"))?"启用":"禁用" %></td>
                                    <td id="opArea" runat="server" data-auth="[{TargetCode:'role',Code:'auth-config'},{TargetCode:'role',Code:'sys-auth-config'},{TargetCode:'role',Code:'mutex'},{TargetCode:'role',Code:'view'},{TargetCode:'role',Code:'edit'},{TargetCode:'role',Code:'delete'}]">
                                        <asp:LinkButton ID="config" CommandName="config" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'role',Code:'auth-config'},{TargetCode:'role',Code:'sys-auth-config'}]">权限</asp:LinkButton>
                                        <asp:LinkButton ID="mutex" CommandName="mutex" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'role',Code:'mutex'}]">互斥</asp:LinkButton>
                                        <asp:LinkButton ID="detail" CommandName="detail" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'role',Code:'view'},{TargetCode:'role',Code:'edit'}]">详细</asp:LinkButton>
                                        <asp:LinkButton ID="delete" CommandName="delete" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'role',Code:'delete'}]">删除</asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>

            </div>
        </div>
        <script src="../../static/jqueryui/js/jqueryui.js"></script>
        <script src="../static/table.js"></script>
    </form>
</body>
</html>
