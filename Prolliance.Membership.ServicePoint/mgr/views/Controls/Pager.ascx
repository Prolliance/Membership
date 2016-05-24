<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="Prolliance.Membership.ServicePoint.Mgr.Views.Controls.Pager" %>
<style>
    
.ui-page-box {
    margin: 20px 0 0 0;
 
}

.ui-page-box, .ui-page-btn {
    color: #777 !important;
}

.ui-page-left p {
    float: left;
    padding: 0 20px 0 0;
}

.ui-page-right {
    float: right;
}

    .ui-page-right a {
        margin: 0 0 0 20px;
    }

</style>
<div class="ui-page-box">
    <div class="ui-page-left">  
        <p>共<asp:label runat="server" ID="totalcount"></asp:label>条记录</p>
        <p>共<asp:label runat="server" ID="totalpages"></asp:label>页</p>
        <p>每页<asp:label runat="server" ID="pagesize"></asp:label>条记录</p>
        <p>当前第<asp:label runat="server" ID="curpage"></asp:label>页</p>
    </div>
    <div class="ui-page-right">
        <asp:LinkButton runat="server" ID="firstpage" OnClick="firstpage_Click" CssClass="ui-page-btn">首页</asp:LinkButton>
        <asp:LinkButton runat="server" ID="previouspage" OnClick="previouspage_Click" CssClass="ui-page-btn">上一页</asp:LinkButton>
        <asp:LinkButton runat="server" ID="nextpage" OnClick="nextpage_Click" CssClass="ui-page-btn">下一页</asp:LinkButton>
        <asp:LinkButton runat="server" ID="lastpage" OnClick="lastpage_Click" CssClass="ui-page-btn">尾页</asp:LinkButton>
    </div>
</div>
