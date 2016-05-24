<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="data-repo.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Install.Step.DataRepo" %>
<div id="<%= this.ClientID %>">
    <h2 class="subject">数据存储</h2>
    <label>数据库类型</label>
    <p>
        <asp:DropDownList ID="dataRepoType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dataRepoType_SelectedIndexChanged">
            <asp:ListItem Value="Prolliance.Membership.DataProvider.SQLServer.DataProviderExtension,Prolliance.Membership.DataProvider.SQLServer">SQL Server(2012/2008/2005)</asp:ListItem>
            <asp:ListItem Value="Prolliance.Membership.DataProvider.MongoDB.DataProvider,Prolliance.Membership.DataProvider.MongoDB">MongoDB</asp:ListItem>
        </asp:DropDownList>
    </p>
    <label>连接字符串</label>
    <p>
        <asp:TextBox ID="connString" runat="server"></asp:TextBox>
    </p>
    <label data-link="el,sqlserver" runat="server">连接示例:</label>
    <p data-link="el,sqlserver" runat="server">
        <i>Data Source=服务器;Database=Membership;User=用户名;Password=密码;MultipleActiveResultSets=True</i><br />
    </p>
    <label data-link="el,sqlserver" runat="server">集成认证示例:</label>
    <p data-link="el,sqlserver" runat="server">
        <i>Data Source=服务器;Initial Catalog=Membership;Integrated Security=True</i><br />
    </p>
    <label data-link="el,mongodb" runat="server">连接示例:</label>
    <p data-link="el,mongodb" runat="server">
        <i>mongodb://服务器|Membership</i>
    </p>
    <label>重要提示:</label>
    <p>
        <i>如果第一次安装 Membership 请在连接字符串指一个不存的全新的数据库名，如果之前安装过再重新安装可以使用原有数据库名。</i>
    </p>
    <br />
    <p>
        <asp:Button ID="next" runat="server" Text="下一步" OnClick="next_Click" />
    </p>
</div>
