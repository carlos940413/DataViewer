<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetails_Ajax.aspx.cs" Inherits="DataViewer_Web.AjaxPage.ProjectDetails_Ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form runat="server">
        <div id="details_content">
            <p>
                所属企业:
                <asp:Label ID="Company_Label" runat="server" Text="Label"></asp:Label>
            </p>
            <p>
                施工单位:
                <asp:Label ID="Team_Label" runat="server" Text="Label"></asp:Label>
            </p>
            <p>
                工期:
                <asp:Label ID="Period_Label" runat="server" Text="Label"></asp:Label>
            </p>
            <asp:HyperLink ID="Details_HyperLink" runat="server">详细信息</asp:HyperLink>
        </div>
    </form>
</body>
</html>
