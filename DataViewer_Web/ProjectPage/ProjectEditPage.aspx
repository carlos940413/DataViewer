<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Project_Master.master" AutoEventWireup="true" CodeBehind="ProjectEditPage.aspx.cs" Inherits="DataViewer_Web.ProjectPage.ProjectEditPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		label {
			margin-top: 5px;
		}

		.for-label {
			text-align: center;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li><a href='<%= project == null ? "/ProjectPage/ProjectPage.aspx" : "/ProjectPage/ProjectDetailsPage.aspx?id=" + project.ID%>'>返回</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server">
		<div class="row-fluid">
			<div class="offset2 span8">
				<div class="well">
					<div class="row-fluid">
						<div class="span2 for-label">
							<label>项目名称:</label>
						</div>
						<div class="span10">
							<asp:TextBox ID="ProjectName_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
					<div class="row-fluid">
						<div class="span2 for-label">
							<label>建设单位:</label>
						</div>
						<div class="span4">
							<asp:DropDownList ID="Company_DropDownList" runat="server" DataTextField="CompanyName" DataValueField="ID" CssClass="input-block-level"></asp:DropDownList>
						</div>
						<div class="span2 for-label">
							<label>施工单位:</label>
						</div>
						<div class="span4">
							<asp:DropDownList ID="Team_DropDownList" runat="server" DataTextField="TeamName" DataValueField="ID" CssClass="input-block-level"></asp:DropDownList>
						</div>
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
