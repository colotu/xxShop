<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SyncUserInfo.aspx.cs" Inherits="YSWL.MALL.Web.Admin.SyncUserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    OnClick="btnSave_Click" class="adminsubmit_short"></asp:Button>
    </div>
    </form>
</body>
</html>
