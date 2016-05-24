<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app-detail.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.AppDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应用详细</title>
    <link href="../static/view.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">应用详细</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <table class="table">
                    <tr>
                        <th>名称</th>
                        <td>
                            <asp:TextBox ID="ctl_Name" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>描述</th>
                        <td>
                            <asp:TextBox ID="ctl_Summary" MaxLength="256" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>状态</th>
                        <td>
                            <asp:RadioButtonList ID="ctl_IsActive" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="True">启用</asp:ListItem>
                                <asp:ListItem Value="False">禁用</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <th>标识</th>
                        <td>
                            <asp:TextBox ID="ctl_Key" MaxLength="32" runat="server" ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>密钥</th>
                        <td>
                            <asp:TextBox ID="ctl_Secret" MaxLength="32" runat="server" ReadOnly="True"></asp:TextBox>
                            <asp:Button ID="resetSecret" data-auth="[{TargetCode:'app',Code:'new'},{TargetCode:'app',Code:'edit'}]" runat="server" Text="重置" OnClick="resetSecret_Click" />
                        </td>
                    </tr>
                    <tr>
                        <th>白名单</th>
                        <td>
                            <asp:TextBox ID="ctl_WhiteList" MaxLength="512" runat="server" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" data-auth="[{TargetCode:'app',Code:'new'},{TargetCode:'app',Code:'edit'}]" runat="server" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
