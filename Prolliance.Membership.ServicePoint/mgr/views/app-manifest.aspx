<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="app-manifest.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.AppManifest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>功能清单</title>
    <link href="../static/view.css" rel="stylesheet" />
    <link href="../../static/jquery-linedtextarea/jquery-linedtextarea.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">功能清单</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <asp:TextBox ID="manifestBox" TextMode="MultiLine" Style="width: 98%; height: 400px;" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" runat="server" data-auth="[{TargetCode:'app',Code:'manifest'}]" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
    <script src="../../static/jquery-linedtextarea/jquery-linedtextarea.js"></script>
    <script>
        (function ($) {
            // $('#manifestBox').linedtextarea();
        }(jQuery));
    </script>
</body>
</html>
