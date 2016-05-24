<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="auth.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Auth" %>

<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Auth | Prolliance Membership</title>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
    <style>
        html, body { overflow: hidden; margin: 0px; padding: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { text-decoration: none; color: #222; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #login-area { background-color: #fcfcfc; position: absolute; top: 50%; left: 50%; border: solid 1px #ddd; width: 270px; padding: 20px; margin-left: -156px; margin-top: -88px; }
        #login-area input[type=text], #login-area input[type=password] { font-family: 微软雅黑; font-size: 15px; padding: 5px; margin: 2px 0px; width: 260px; border: solid 1px #ccc; }
        #login-area .title { padding-bottom: 10px; font-weight: bold; font-size: 15px; }
        #login-area input[type=submit] { margin: 0px; margin-top: 10px; padding: 4px 20px; font-family: 微软雅黑; }
        #version { font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
</head>
<body>
    <div id="header">
        MEMBERSHIP AUTH
       <label id="version">version <%= Global.Version %></label>
    </div>
    <form id="aspForm" runat="server">
        <div id="login-area">
            <div class="title">MEMBERSHIP AUTH</div>
            <div>
                <asp:TextBox ID="accountBox" autocomplete="off" runat="server" TextMode="SingleLine" placeholder="帐号"></asp:TextBox>
                <asp:TextBox ID="passwordBox" autocomplete="off" runat="server" TextMode="password" placeholder="密码"></asp:TextBox>
            </div>
            <div>
                <asp:Button ID="loginButton" runat="server" Text="登录" OnClick="loginButton_Click" />
            </div>
        </div>
    </form>
</body>
</html>
