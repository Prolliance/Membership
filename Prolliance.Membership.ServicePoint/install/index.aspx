<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Index" %>

<%@ Import Namespace="Prolliance.Membership.ServicePoint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>安装 | Prolliance Membership</title>
    <script>
        if (self != top) {
            top.location.href = self.location.href;
        }
    </script>
    <style>
        * { font-family: 微软雅黑, Verdana; }
        body { margin: 0px; color: #333; font-family: 微软雅黑, Verdana; font-size: 13px; }
        a { color: blue; }
        a:hover { text-decoration: underline; }
        #header, #footer { padding: 8px; background-color: #333; color: #fdfdfd; }
        #header a, #footer a { color: #fdfdfd; }
        #header { font-size: 22px; font-weight: bold; }
        #menu li { padding: 5px 0px; }
        .subject { font-weight: bold; margin: 0px; font-size: 20px; padding: 10px 0px; }
        .content { padding: 10px; }
        input[type=text], input[type=password], select, textarea { width: 650px; box-sizing: border-box; padding: 3px; border: solid 1px #aaa; }
        input[type=button], input[type=submit], button { padding: 3px 15px; }
        label { font-size: 14px; color: #111; display: block; }
        p { margin: 3px 0px 15px 0px; padding: 0px; }
        #version { display: inline; font-size: 12px; font-weight: normal; color: #eee; font-style: italic; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server" data-role="grid" data-grid-rows="51,*,41" data-grid-cols="*">
        <div id="header">
            安装 Membership
            <label id="version">version <%= Global.Version %></label>
        </div>
        <asp:Panel ID="stepArea" CssClass="content" runat="server"></asp:Panel>
    </form>
    <script>
        (function ($) {
            var area = $('#stepArea');
            area.on("click", "input[type=submit]", function () {
                $("input[type=text],input[type=password],input[type=submit],select,button,textarea").attr('disabled', 'disabled');
                $(this).val("Loading...");
            });
        }(jQuery));
    </script>
</body>
</html>

