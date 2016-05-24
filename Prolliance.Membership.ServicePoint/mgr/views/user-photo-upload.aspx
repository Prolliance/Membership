<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user-photo-upload.aspx.cs" Inherits="Prolliance.Membership.ServicePoint.mgr.views.UserPhotoUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户头像</title>
    <link href="../static/view.css" rel="stylesheet" />
    <style>
        #nav {
            padding: 0px;
            background: #eee;
            margin: 0px;
            border-bottom: solid 1px #ccc;
            border-top: solid 1px #ccc;
        }

            #nav li {
                display: inline-block;
                padding: 10px;
                position: relative;
            }
            #nav li a {
                display: inline-block;
                width: 100%;
                height: 100%;
                cursor: pointer;
            }

            #nav li.selected {
                background: #d5d5d5 !important;
            }
    </style>
</head>
<body>
    <form id="aspForm" runat="server" enctype="multipart/form-data">
        <div id="basepanel" class="dialog-content">
            <div class="view" style="text-align:center;vertical-align:middle;">
                <table class="table">
                    <tr>
                        <asp:Image ID="imgPhoto" runat="server" Width="100" Height="100" />
                    </tr>
                </table>
            </div>
        </div>
        <div class="dialog-footer">
            <div class="view">
                <asp:FileUpload ID="filPhoto" runat="server" />
                <asp:Button ID="save" runat="server" Text="上传" data-auth="[{TargetCode:'user',Code:'new'},{TargetCode:'user',Code:'edit'}]" OnClick="save_Click" />
            </div>
        </div>
    </form>
</body>
</html>
