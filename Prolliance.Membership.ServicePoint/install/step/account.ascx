<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="account.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Step.Account" %>
<div id="<%=this.ClientID %>">
    <h2 class="subject">初始管理员</h2>
    <label>账号</label>
    <p>
        <asp:TextBox ID="account" runat="server"></asp:TextBox>
    </p>
    <label data-link="password" runat="server">密码</label>
    <p data-link="password" runat="server">
        <asp:TextBox ID="password" TextMode="Password" runat="server"></asp:TextBox>
    </p>
    <label runat="server">重要提示:</label>
    <p data-link="dc" runat="server">
        <i>如果先择了 “基于 AD 的身份认证” ，请指定一个 “已存在的域账号” ，基于 “数据库的身份认证”，请设定 “新的账号和密码”。</i>
    </p>
    <br />
    <p>
        <asp:Button ID="next" runat="server" Text="下一步" OnClick="next_Click" />
    </p>
</div>
