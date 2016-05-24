<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="extensioninfo.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.extensioninfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>应用列表</title>
    <link href="../static/view.css" rel="stylesheet" />
    <link href="../../static/jqueryui/css/jquery-ui.css" rel="stylesheet" />
    <style>
        .selectTB {
            width: 200px;   
        }
    </style>
</head>
<body>
    <form id="aspForm" class="content" runat="server">
        <div class="view">
            <div class="toolbar">
              <asp:DropDownList CssClass="selectTB" ID="dropSelectTB" runat="server">
                  <asp:ListItem Value="User">请选择</asp:ListItem>
                  <asp:ListItem Value="User">用户表</asp:ListItem>
                  <asp:ListItem Value="Organization">组织结构表</asp:ListItem>
                  
              </asp:DropDownList><asp:Button ID="btnSubmit" runat="server" Text="确认" OnClick="btnSubmit_Click1" />
               &nbsp; &nbsp;&nbsp;<asp:Button ID="btnNew" runat="server" Text="新建字段" OnClick="btnNew_Click1" />
            
            </div>
            <div id="tableArea" runat="server">
               
                <table class="table">
                    <thead>
                        <tr>
                            <th style="width: 200px;">字段名称</th>
                            <th>描述</th>
                 
                            <%--<th style="width: 150px;" id="opArea" runat="server" data-auth="[{TargetCode:'app',Code:'manifest'},{TargetCode:'app',Code:'edit'},{TargetCode:'app',Code:'view'},{TargetCode:'app',Code:'delete'}]">操作</th>--%>
                        </tr>
                    </thead>
                    <tbody class="sortable-list">
                        <asp:Repeater ID="dataList" runat="server" >
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("Key")%></td>
                                    <td><%#Eval("value")%></td>
                                   
                                   <%-- <td id="opArea" runat="server" data-auth="[{TargetCode:'app',Code:'manifest'},{TargetCode:'app',Code:'edit'},{TargetCode:'app',Code:'view'},{TargetCode:'app',Code:'delete'}]">
                                        <asp:LinkButton ID="manifest" CommandName="manifest" CommandArgument='<%# Eval("Id") %>' runat="server" data-auth="[{TargetCode:'app',Code:'manifest'}]">功能清单</asp:LinkButton>
                                      
                                    </td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <script src="../../static/jqueryui/js/jqueryui.js"></script>
        <script src="../static/table.js"></script>
    </form>
</body>
</html>
