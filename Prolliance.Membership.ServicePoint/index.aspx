<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Index" %>

<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Readme | Prolliance Membership</title>
    <style>
        body { margin: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { color: blue; }
        a:hover { text-decoration: underline; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #menu li { padding: 5px 0px; }
        #version { font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
</head>
<body>
    <form id="aspForm" runat="server" data-role="grid" data-grid-rows="51,*,41" data-grid-cols="*">
        <div id="header">
            Prolliance Membership
            <label id="version">version <%= Global.Version %></label>
        </div>
        <ul id="menu">
            <li><a href="./mgr">Membership 控制台</a></li>
            <li><a href="./service">直接访问 Web 服务</a></li>
            <li><a href="./files/csharp-sdk.zip">下载 CSharp-SDK</a></li>
            <li><a href="./files/csharp-sdk-demo.zip">下载 CSharp-SDK-Demo</a></li>
            <li><a href="./files/csharp-sdk-doc.zip">下载 CSharp-SDK-Doc</a></li>
        </ul>
    </form>
</body>
</html>
