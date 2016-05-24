<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="position-report-to.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.mgr.views.PositionReportTo" %>

<%@ Import Namespace="Prolliance.Membership.Business" %>
<%@ Register Src="~/mgr/views/Controls/OrgTree.ascx" TagPrefix="uc1" TagName="OrgTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>汇报关系</title>
    <link href="../static/view.css" rel="stylesheet" />
    <style>
        .inner-view { width: 100%; height: 100%; overflow: auto; }
    </style>
</head>
<body>
    <form id="aspForm" runat="server">
        <div class="dialog-header">
            <div class="view">汇报关系</div>
        </div>
        <div class="dialog-content">
            <table style="height: 100%; width: 100%;">
                <tr>
                    <td style="width: 50%; vertical-align: top; border-right: solid 1px #ddd;">
                        <div class="inner-view">
                            <uc1:OrgTree runat="server" ID="OrgTree" />
                        </div>
                    </td>
                    <td style="width: 50%; vertical-align: top;">
                        <div class="inner-view">
                            <div id="tableArea" runat="server" style="padding: 10px;">
                                <table class="table">
                                    <tr>
                                        <th style="width: 23px;"></th>
                                        <th>名称</th>
                                        <th style="width: 35px;">状态</th>
                                        <th style="width: 35px;">操作</th>
                                    </tr>
                                    <asp:Repeater ID="dataList" runat="server" OnItemCommand="dataList_ItemCommand">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="check" Checked="<%# this.SelectedList.Contains(((Position)Container.DataItem).Id) %>" runat="server" Enabled="false" />
                                                </td>
                                                <td><%# Eval("Name") %></td>
                                                <td><%# Convert.ToBoolean(Eval("IsActive"))?"启用":"禁用" %></td>
                                                <td>
                                                    <asp:LinkButton ID="btn" CommandArgument='<%# Eval("Id") %>' CommandName='<%# this.SelectedList.Contains(((Position)Container.DataItem).Id)?"cancel":"select" %>' runat="server"><%# this.SelectedList.Contains(((Position)Container.DataItem).Id)?"取消":"选择" %></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 100px; border-top: solid 1px #ddd;">
                        <div class="inner-view">
                            <div id="selectedDataListArea" runat="server" style="padding: 10px;">
                                <asp:Repeater ID="selectedDataList" runat="server">
                                    <ItemTemplate>
                                        <label><%# Container.DataItem %></label>
                                    </ItemTemplate>
                                    <SeparatorTemplate>
                                        , 
                                    </SeparatorTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:Button ID="save" runat="server" data-auth="[{TargetCode:'position',Code:'new'},{TargetCode:'position',Code:'edit'}]" Text="保存" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
