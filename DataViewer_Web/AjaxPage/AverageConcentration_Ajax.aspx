<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AverageConcentration_Ajax.aspx.cs" Inherits="DataViewer_Web.AjaxPage.AverageConcentration_Ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<link type="text/css" href="../bootstrap/css/bootstrap.css" rel="stylesheet" />
	<script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.js"></script>
	<script type="text/javascript" src="../bootstrap/js/bootstrap.js"></script>
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div id="details_content" data-spy="scroll">
			<div class="tabbable">
				<ul class="nav nav-tabs">
					<asp:ListView ID="Area_ListView" runat="server">
						<ItemTemplate>
							<li class='<%# Container.DataItemIndex==0?"active":"" %>'><a href='<%# "#"+Eval("ID")%>' data-toggle="tab"><%# Eval("AreaName") %></a></li>
						</ItemTemplate>
					</asp:ListView>
				</ul>
				<div class="tab-content">
					<asp:ListView ID="Concentrations_ListView" runat="server">
						<ItemTemplate>
							<div class='<%# Container.DataItemIndex==0?"tab-pane active":"tab-pane" %>' id='<%# Eval("Key") %>'>
								<asp:GridView runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" DataSource='<%# Eval("Value") %>'>
									<Columns>
										<asp:BoundField DataField="Value" DataFormatString="{0:0.00}/mL" HeaderText="浓度" />
										<asp:BoundField DataField="Key" DataFormatString="{0:yyyy}年{0:MM}月{0:dd}日 {0:HH}:{0:mm}:{0:ss}" HeaderText="采集时间" />
									</Columns>
								</asp:GridView>
							</div>
						</ItemTemplate>
					</asp:ListView>
				</div>
			</div>
		</div>
	</form>
</body>
</html>
