<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="role-operation.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.RoleOperation" %>

<%@ Import Namespace="Prolliance.Membership.Business" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>权限配置</title>
    <link href="../static/view.css" rel="stylesheet" />
    <style>
        .dialog-content .app { color: #555; font-size: 16px; margin: 0px; padding: 0px; }
        .dialog-content .group { color: #777; font-size: 14px; font-weight: bold; padding: 5px; margin: 5px 0px; background-color: #f5f5f5; }
        .dialog-content .table { margin-top: 5px; }
        .dialog-content input { vertical-align: text-bottom; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">权限配置</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <asp:Repeater ID="appList" runat="server" OnItemDataBound="appList_ItemDataBound">
                    <ItemTemplate>
                        <h1 class="app">
                            <%#Eval("Name") %></h1>
                        <asp:Repeater ID="groupList" runat="server">
                            <ItemTemplate>
                                <div class="group">
                                    <%# Eval("key") %>
                                    </th>
                                </div>
                                <asp:Repeater ID="targetList" runat="server">
                                    <ItemTemplate>
                                        <table class="table">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        <asp:CheckBox ID="check" runat="server" data-link='<%# Eval("Id") %>' Text='<%# Eval("Name") %>' />
                                                        : <%#Eval("Summary") %>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody data-link='<%# Eval("Id") %>'>
                                                <tr>
                                                    <td>
                                                        <asp:Repeater ID="operationList" runat="server">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="check" data-id='<%#Eval("Id") %>' Checked="<%# Check((Operation)(Container.DataItem)) %>" runat="server" Text='<%# Eval("Name") %>' />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ItemTemplate>
                        </asp:Repeater>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <input id="saveButton" type="button" data-auth="[{TargetCode:'role',Code:'auth-config'},{TargetCode:'role',Code:'sys-auth-config'}]" value="保存" />
            </div>
        </div>
    </form>
    <script>
        (function ($) {
            var saveButton = $("#saveButton");
            var operationItems = $('[data-id]');
            saveButton.on('click', function () {
                var giveIdList = [];
                var cancelIdList = [];
                operationItems.each(function (i, item) {
                    var itemBox = $(item);
                    if (itemBox.prop('checked')
                        || itemBox.find('input').prop('checked')) {
                        giveIdList.push(itemBox.attr("data-id"));
                    } else {
                        cancelIdList.push(itemBox.attr("data-id"));
                    }
                });
                Server.Save(giveIdList, cancelIdList);
            });
            var linkItems = $('[data-link] input[type=checkbox]');
            linkItems.on('change', function () {
                var itemBox = $(this);
                var linkBoxs = $("[data-link=" + itemBox.parent().attr('data-link') + "] input[type=checkbox]");
                linkBoxs.prop('checked', itemBox.prop('checked'));
            });
        }(jQuery));
    </script>
</body>
</html>
