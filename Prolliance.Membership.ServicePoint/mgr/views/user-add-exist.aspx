<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user-add-exist.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.UserAddExtist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户查找</title>
    <link href="../static/view.css" rel="stylesheet" />
    <script>
        function loadUserClick(btn) {
            $(btn).attr('disabled', 'disabled').val('同步中...');
        }
    </script>
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">用户查找</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <div class="toolbar">
                    <asp:TextBox ID="ctl_keyword" runat="server"></asp:TextBox>
                    <asp:Button ID="select" runat="server" Text="搜索" OnClick="find_Click" />
                    <asp:Button ID="loadRemote" runat="server" OnClientClick="loadUserClick(this);" Width="80px" Text="同步用户" OnClick="loadRemote_Click" />
                </div>
                <div id="tableArea" runat="server">
                    <table class="table">
                        <tr>
                            <th style="width: 20px;"></th>
                            <th style="width: 150px;">姓名</th>
                            <th style="width: 150px;">帐号</th>
                            <th>邮箱</th>
                        </tr>
                        <asp:Repeater ID="dataList" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="userId" runat="server" data-id='<%# Eval("Id")%>' /></td>
                                    <td><%# Eval("Name") %></td>
                                    <td><%# Eval("Account")%></td>
                                    <td><%# Eval("Email") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
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
