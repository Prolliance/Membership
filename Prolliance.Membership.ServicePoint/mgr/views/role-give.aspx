<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role-give.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.RoleGive" %>

<%@ Import Namespace="Prolliance.Membership.Business" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>角色授予</title>
    <link href="../static/view.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">角色授予</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <div id="tableArea" style="height: 100%; overflow-y: auto;" runat="server">
                    <table class="table">
                        <tr>
                            <th style="width: 20px;"></th>
                            <th style="width: 150px;">编码</th>
                            <th style="width: 150px;">名称</th>
                            <th>描述</th>
                        </tr>
                        <asp:Repeater ID="dataList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="roleId" Checked="<%# Check((Role)(Container.DataItem)) %>" runat="server" data-id='<%# Eval("Id")%>' /></td>
                                    <td><%# Eval("Code") %></td>
                                    <td><%# Eval("Name")%></td>
                                    <td><%# Eval("Summary") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" runat="server" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
