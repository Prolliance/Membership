<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Index" %>

<%@ Import Namespace="Prolliance.Membership.Common" %>
<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Control Panel | Prolliance Membership</title>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
    <link href="../static/common.css" rel="stylesheet" />
    <style>
        html, body { background-color: #333; overflow: hidden; margin: 0px; padding: 0px; color: #222; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { text-decoration: none; color: #222; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #nav { padding: 0px; background: #eee; margin: 0px; border-bottom: solid 1px #ccc; border-top: solid 1px #ccc; }
        #nav li { display: inline-block; padding: 10px; position: relative; }
        #nav li a { display: inline-block; width: 100%; height: 100%; cursor: pointer; }
        #nav li.selected { background: #d5d5d5 !important; }
        #frame-area { height: 100%; position: relative; background: #fff; }
        #frame { width: 100%; height: 100%; border: none; margin: 0px; }
        #info-panel { display: inline; float: right; font-size: 14px; padding: 5px; }
        #version { font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server">
        <div id="header">
            Membership Control Panel
                        <label id="version">version <%= Global.Version %></label>
            <div id="info-panel">
                <span id="gobackArea" runat="server">
                    <asp:LinkButton ID="goback" runat="server" OnClick="goback_Click">返回</asp:LinkButton>
                    |
                </span>
                <asp:LinkButton ID="username" runat="server">用户名</asp:LinkButton>
                | 
                <asp:LinkButton ID="logout" runat="server" OnClick="logout_Click">登出</asp:LinkButton>
            </div>
        </div>
        <ul id="nav">
            <li id="app" data-auth="[{TargetCode:'app',Code:'*'}]" runat="server"><a href="#views/app-list.aspx">应用管理</a></li>
            <li id="role" data-auth="[{TargetCode:'role',Code:'*'}]" runat="server"><a href="#views/role-list.aspx">角色管理</a></li>
            <li id="organization" data-auth="[{TargetCode:'organization',Code:'*'}]" runat="server"><a href="#views/org-list.aspx">组织管理</a></li>
            <li id="position" data-auth="[{TargetCode:'position',Code:'*'}]" runat="server"><a href="#views/position-list.aspx">职位管理</a></li>
            <li id="user" data-auth="[{TargetCode:'user',Code:'*'}]" runat="server"><a href="#views/user-list.aspx">人员管理</a></li>
            <li id="Li1" data-auth="[{TargetCode:'user',Code:'*'}]" runat="server"><a href="#views/extensioninfo.aspx">扩展信息</a></li>
        </ul>
    </form>
    <div id="frame-area">
        <iframe id="frame" name="frame" frameborder="0"></iframe>
    </div>
    <script src="../static/jquery.js"></script>
    <script src="../static/jquery.xhashchange.js"></script>
    <script>
        (function ($) {
            var baseUrl = location.href.toLowerCase().split('?')[0].split('#')[0].replace("frame.aspx", "");
            var frameArea = $('#frame-area');
            var frame = $("#frame");
            var nav = $("#nav");
            var header = $("#header");
            var navItems = $("#nav li");
            //处理Hash
            var handleHash = function () {
                var hash = location.hash.replace('#', '').toLowerCase();
                var _url = frame[0].contentWindow.location.href.toLowerCase().replace(baseUrl, "");
                if (hash != _url) {
                    frame.attr('src', hash);
                }
                navItems.each(function () {
                    var active = $(this).find('a').attr('href') == "#" + hash;
                    $(this)[active ? "addClass" : "removeClass"]("selected");
                });
            };
            frame.on('load', function () {
                var _url = frame[0].contentWindow.location.href.toLowerCase().replace(baseUrl, "");
                location.hash = "#" + _url;
            });
            $(window).on("hashchange", handleHash);
            //默认页
            if (location.hash == null || location.hash == "") {
                location.hash = ($(navItems[0]).find("a").attr('href') || "").toLowerCase();
            } else {
                handleHash();
            }
            //处理布局
            var handleLayout = function () {
                frameArea.height($(window).height() - nav.outerHeight() - header.outerHeight() - 3);
            };
            $(window).resize(handleLayout);
            handleLayout();
        }(jQuery));
    </script>
</body>
</html>
