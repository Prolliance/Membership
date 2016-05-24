<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="init-data.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Step.InitData" %>
<div id="<%=this.ClientID %>">
    <h2 class="subject">初始化数据</h2>
    <p>
        请单击 “下一步”，将进行安装并初始化数据，此步骤将需要若干分钟...
    </p>
    <br />
    <p>
        <asp:Button ID="next" runat="server" Text="下一步" OnClick="next_Click" />
    </p>
</div>
