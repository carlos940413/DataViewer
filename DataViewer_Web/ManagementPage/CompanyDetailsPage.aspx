<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Management_Company_Master.Master" AutoEventWireup="true" CodeBehind="CompanyDetailsPage.aspx.cs" Inherits="DataViewer_Web.ManagementPage.CompanyDetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li><a href='<%="/ManagementPage/CompanyEditPage.aspx?id="+company.ID%>'>修改建设单位信息</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server">
		<div class="row-fluid">
			<div class="offset2 span8">
				<div class="row-fluid">
					<div class="well">
						<div class="row-fluid">
							<div class="span12">
								建设单位名称:　<%=company.CompanyName %>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
