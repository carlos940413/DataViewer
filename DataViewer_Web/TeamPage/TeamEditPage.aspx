<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Team_Master.master" AutoEventWireup="true" CodeBehind="TeamEditPage.aspx.cs" Inherits="DataViewer_Web.TeamPage.TeamEditPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.well > div > input {
			margin-bottom: 0px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li><a href='<%= team==null?"/TeamPage/TeamPage.aspx":"/TeamPage/TeamDetailsPage.aspx?id="+team.ID%>'>返回</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server">
		<div class="row-fluid">
			<div class="offset2 span8">
				<div class="well">
					<div class="row-fluid">
						<span>施工单位名称:</span>
						<asp:TextBox ID="TeamName_TextBox" runat="server"></asp:TextBox>
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
