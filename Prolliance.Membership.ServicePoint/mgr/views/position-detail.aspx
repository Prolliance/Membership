<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="position-detail.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.PositionDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>职位详细</title>
    <link href="../static/view.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">职位详细</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <table class="table">
                    <tr>
                        <th>编码</th>
                        <td>
                            <asp:TextBox ID="ctl_Code" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>名称</th>
                        <td>
                            <asp:TextBox ID="ctl_Name" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>类型</th>
                        <td>
                            <asp:TextBox ID="ctl_Type" MaxLength="32" runat="server"></asp:TextBox></td>
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
                </table>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" runat="server" data-auth="[{TargetCode:'position',Code:'new'},{TargetCode:'position',Code:'edit'}]" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
