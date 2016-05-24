<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="done.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Step.Done" %>
<div id="<%=this.ClientID %>">
    <h2 class="subject">安装信息</h2>
    <p>
        安装完成，现在即可使用初始管理员登录...
    </p>
    <br />
    <p>
        <asp:Button ID="next" runat="server" Text="进入主页" OnClick="next_Click" />
    </p>
</div>
