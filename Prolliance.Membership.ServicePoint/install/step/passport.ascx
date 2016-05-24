<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="passport.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Step.Passport" %>
<div id="<%= this.ClientID %>">
    <h2 class="subject">身份认证</h2>
    <label>认证方式</label>
    <p>
        <asp:DropDownList ID="authType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="authType_SelectedIndexChanged">
            <asp:ListItem Value="Prolliance.Membership.Passport.Database.PassportProvider,Prolliance.Membership.Passport.Database">基于数据库</asp:ListItem>
            <asp:ListItem Value="Prolliance.Membership.Passport.DC.PassportProvider,Prolliance.Membership.Passport.DC">基于域控制器</asp:ListItem>
        </asp:DropDownList>
    </p>
    <label runat="server" data-link="dc">域控地址</label>
    <p runat="server" data-link="dc">
        <asp:TextBox ID="dc" runat="server"></asp:TextBox>
    </p>
    <label runat="server" data-link="dc">代理账号</label>
    <p runat="server" data-link="dc">
        <asp:TextBox ID="administrator" runat="server"></asp:TextBox>
    </p>
    <label runat="server" data-link="dc">代理密码</label>
    <p runat="server" data-link="dc">
        <asp:TextBox ID="password" runat="server"></asp:TextBox>
    </p>
    <label data-link="dc" runat="server">重要提示:</label>
    <p data-link="dc" runat="server">
        <i>如果需要使用 Cotnrol Panel 中的 “重置用户密码” 功能，需要配置一个 “有管理权限的域账号”</i>
    </p>
    <br />
    <p>
        <asp:Button ID="next" runat="server" Text="下一步" OnClick="next_Click" />
    </p>
</div>
