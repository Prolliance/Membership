<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="expr.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Dev.Expr" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>表达式编辑器 | Prolliance Membership</title>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
    <style>
        html, body { overflow: hidden; margin: 0px; padding: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        .label { padding: 5px 0px; font-size: 12px; color: #444; }
    </style>
</head>
<body>
    <div id="header">
        表达式编辑器
    </div>
    <form id="aspForm" runat="server">
        <div style="padding: 8px;">
            <asp:TextBox ID="boxExpr" runat="server" Height="220px" TextMode="MultiLine" Width="600px"></asp:TextBox>
            <br />
            <asp:Button ID="btnExec" runat="server" Text="执行表达式" OnClick="btnExec_Click" />
            <asp:Button ID="btnClearSpace" runat="server" Text="清理换行符" OnClick="btnClearSpace_Click" />
            <br />
            <asp:TextBox ID="boxResult" runat="server" Height="220px" TextMode="MultiLine" Width="600px"></asp:TextBox>
            <br />
            <asp:Label ID="lblTime" runat="server" CssClass="label" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
