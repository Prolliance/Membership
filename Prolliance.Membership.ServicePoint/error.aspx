<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Error" %>

<%@ Import Namespace="Prolliance.Membership.Common" %>
<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Error | Prolliance Membership</title>
    <style>
        body { margin: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { color: blue; }
        a:hover { text-decoration: underline; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #menu li { padding: 5px 0px; }
        #subject { font-weight: normal; font-size: 20px; padding: 0px 15px; }
        #version { font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server" data-role="grid" data-grid-rows="51,*,41" data-grid-cols="*">
        <div id="header">
            Prolliance Membership
                    <label id="version">version <%= Global.Version %></label>
        </div>
        <h1 id="subject">抱歉，好像遇到了一些问题...</h1>
        <ul id="menu">
            <li>已经收集问题的详细信息</li>
            <li>您可以稍后重试您的操作 <a href='<%=this.ResolveUrl("~/") %>'>回到主页</a></li>
            <li>或者立刻联系 <a href="mailto:<%= AppSettings.Email %>"><%= AppSettings.Email %></a></li>
        </ul>
    </form>
    <script>
        (function ($) {
            if (self != top) {
                $("#header").hide();
            }
        }(jQuery));
    </script>
</body>
</html>
