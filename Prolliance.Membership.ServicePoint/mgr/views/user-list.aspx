<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user-list.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.UserList" %>

<%@ Register Src="~/mgr/views/Controls/OrgTree.ascx" TagPrefix="uc1" TagName="OrgTree" %>
<%@ Register Src="~/mgr/views/Controls/Pager.ascx" TagPrefix="uc1" TagName="Pager" %>

<%@ Import Namespace="Prolliance.Membership.Business" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户列表</title>
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
                        <asp:Button ID="new" runat="server" data-auth="[{TargetCode:'user',Code:'new'}]" Text="新建用户" OnClick="new_Click" />
                        <asp:Button ID="add" runat="server" data-auth="[{TargetCode:'user',Code:'add'}]" Text="添加现有用户" OnClick="add_Click" />
                        <asp:TextBox  ID="txtSearch" Width="250px" runat="server"></asp:TextBox><asp:Button ID="Button1" runat="server" Text="搜索" OnClick="Button1_Click" />
                    </div>
                    <div id="tableArea" runat="server">
                        <table class="table">
                            <thead>
                                <tr>
                                    <th style="width: 100px;">名称</th>
                                    <th>职位</th>
                                    <th style="width: 45px;">状态</th>
                                    <th style="width: 180px;" id="opArea" runat="server" data-auth="[{TargetCode:'user',Code:'role'},{TargetCode:'user',Code:'edit'},{TargetCode:'user',Code:'view'},{TargetCode:'user',Code:'password'},{TargetCode:'user',Code:'remove'},{TargetCode:'user',Code:'delete'}]">操作</th>
                                </tr>
                            </thead>
                            <tbody class="sortable-list">
                                <asp:Repeater ID="dataList" runat="server" OnItemCommand="dataList_ItemCommand">
                                    <ItemTemplate>
                                        <tr data-id="<%# Eval("Id") %>">
                                            <td><%# Eval("Name") %></td>
                                            <td><%# GetPositionNameList((User)(Container.DataItem)) %></td>
                                            <td><%# Convert.ToBoolean(Eval("IsActive"))?"启用":"禁用" %></td>
                                            <td id="opArea" runat="server" data-auth="[{TargetCode:'user',Code:'role'},{TargetCode:'user',Code:'edit'},{TargetCode:'user',Code:'view'},{TargetCode:'user',Code:'password'},{TargetCode:'user',Code:'remove'},{TargetCode:'user',Code:'delete'}]">
                                                <asp:LinkButton ID="role" CommandName="role" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'user',Code:'role'}]">角色</asp:LinkButton>
                                                <asp:LinkButton ID="detail" CommandName="detail" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'user',Code:'edit'},{TargetCode:'user',Code:'view'}]">详细</asp:LinkButton>
                                                <asp:LinkButton ID="photo" CommandName="photo" CommandArgument='<%# Eval("Account") %>' runat="server" data-auth="[{TargetCode:'user',Code:'edit'},{TargetCode:'user',Code:'view'}]">头像</asp:LinkButton>
                                                <asp:LinkButton ID="password" CommandName="password" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'user',Code:'password'}]">密码</asp:LinkButton>
                                                <asp:LinkButton ID="remove" CommandName="remove" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'user',Code:'remove'}]">移除</asp:LinkButton>
                                                <asp:LinkButton ID="delete" CommandName="delete" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'user',Code:'delete'}]">删除</asp:LinkButton>

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <uc1:Pager runat="server" PageSize="20" OnPaging="Pager1_Paging" id="Pager" />
                    </div>
                </div>
            </div>
        </div>
        <script src="../../static/jqueryui/js/jqueryui.js"></script>
        <script src="../static/table.js"></script>
    </form>
</body>
</html>
