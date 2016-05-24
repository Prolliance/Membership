<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Service.Index" %>

<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>服务索引 | Prolliance Membership</title>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
    <style>
        html, body { overflow: hidden; margin: 0px; padding: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { color: blue; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #menu li { padding: 5px 0px; }
        #version { font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
</head>
<body>
    <div id="header">
        服务索引
        <label id="version">version <%= Global.Version %></label>
    </div>
    <form id="aspForm" runat="server" data-role="grid" data-grid-rows="51,*,41" data-grid-cols="*">
        <ul id="menu">
            <li><a href="app.ashx">应用服务</a></li>
            <li><a href="user.ashx">用户服务</a></li>
            <li><a href="organization.ashx">组织服务</a></li>
            <li><a href="position.ashx">职位服务</a></li>
            <li><a href="role.ashx">角色服务</a></li>
        </ul>
    </form>
</body>
</html>
