<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Management_Team_Master.master" AutoEventWireup="true" CodeBehind="TeamDetailsPage.aspx.cs" Inherits="DataViewer_Web.ManagementPage.TeamDetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li><a href='<%= "/ManagementPage/TeamEditPage.aspx?id="+team.ID%>'>修改施工单位</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server">
		<div class="row-fluid">
			<div class="offset2 span8">
				<div class="row-fluid">
					<div class="well">
						<div class="row-fluid">
							<div class="span12">
								施工单位名称:　<%=team.TeamName%>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
