<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageConcentration_Ajax.aspx.cs" Inherits="DataViewer_Web.AjaxPage.AverageConcentration_Ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="details_content">
            <asp:GridView ID="Concentration_GridView" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="Value" DataFormatString="{0}/mL" HeaderText="浓度"/>
                    <asp:BoundField DataField="Key" DataFormatString="{0:yyyy}年{0:MM}月{0:dd}日 {0:HH}:{0:mm}:{0:ss}" HeaderText="采集时间"/>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
