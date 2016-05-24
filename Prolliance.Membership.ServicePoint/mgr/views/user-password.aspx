<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user-password.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.UserPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户密码</title>
    <link href="../static/view.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">用户密码</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <table class="table">
                    <tr>
                        <th>账号</th>
                        <td>
                            <asp:TextBox ID="ctl_Account" MaxLength="32" ReadOnly="true" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>密码</th>
                        <td>
                            <asp:TextBox ID="ctl_Password1" TextMode="Password" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>再输一次</th>
                        <td>
                            <asp:TextBox ID="ctl_Password2" TextMode="Password" MaxLength="256" runat="server"></asp:TextBox></td>
                    </tr>
                </table>
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
