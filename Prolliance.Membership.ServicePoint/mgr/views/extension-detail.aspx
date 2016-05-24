<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="extension-detail.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.ExtensionDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>组织详细</title>
    <link href="../static/view.css" rel="stylesheet" />
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">新建字段信息</div>
        </div>
        <div class="dialog-content">
            <div class="view">
                <table class="table">
                    <tr>
                        <th>字段名称</th>
                        <td>
                            <asp:TextBox ID="ctl_ColumnName" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>字段描述</th>
                        <td>
                            <asp:TextBox ID="ctl_Des" MaxLength="32" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                  
                    <tr>
                        <th>字段类型</th>
                        <td>
                          
                            <asp:RadioButtonList ID="ctl_Type" runat="server"  RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="ctl_Type_SelectedIndexChanged">
                            
                                <asp:ListItem Value="1" Selected="True">文本</asp:ListItem>
                                 <asp:ListItem Value="0" >数值</asp:ListItem>
                                <asp:ListItem Value="2">日期</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                       <tr>
                        <th><asp:Literal ID="litLen" Text="字段长度或数字精度" runat="server"></asp:Literal></th>
                        <td>
                            <asp:TextBox ID="ctl_Length" MaxLength="32" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>默认值</th>
                        <td>
                            <asp:TextBox ID="ctl_Defaultval" MaxLength="32" runat="server"></asp:TextBox></td>
                    </tr>
                    </table>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" runat="server" data-auth="[{TargetCode:'organization',Code:'new'},{TargetCode:'organization',Code:'edit'}]" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
