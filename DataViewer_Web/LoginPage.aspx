<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="DataViewer_Web.LoginPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<link type="text/css" href="bootstrap/css/bootstrap.css" rel="stylesheet" />
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<div style="margin-top: 200px; text-align: center">
			<div>
				<h1>施 工 项 目 管 理 平 台</h1>
			</div>
			<div>
				<div>
					<asp:Label runat="server" Text="用户名" Width="50"></asp:Label>
					<asp:TextBox ID="Username_TextBox" runat="server"></asp:TextBox>
				</div>
				<div>
					<asp:Label ID="Label1" runat="server" Text="密码" Width="50"></asp:Label>
					<asp:TextBox ID="Password_TextBox" runat="server" TextMode="Password"></asp:TextBox>
				</div>
			</div>
			<div>
				<asp:Button runat="server" Text="登录" CssClass="btn" Width="100"/>
			</div>
		</div>
	</form>
</body>
</html>
