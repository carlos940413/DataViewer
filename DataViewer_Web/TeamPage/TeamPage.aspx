<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Team_Master.master" AutoEventWireup="true" CodeBehind="TeamPage.aspx.cs" Inherits="DataViewer_Web.TeamPage.TeamPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
	 #<%=Search_Button.ClientID%>{
		margin-top:0px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="AddTeam_HyperLink" runat="server">添加施工单位</asp:HyperLink>
	</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server" class="form-search">
		<div class="row-fluid">
			<div class="input-append row-fluid span8 offset2">
				<asp:TextBox ID="TeamName_TextBox" runat="server" CssClass="search-query span10"></asp:TextBox>
				<asp:Button ID="Search_Button" runat="server" Text="搜索" CssClass="btn span2" OnClick="On_SearchButton_Click" />
			</div>
		</div>
		<div class="row-fluid" style="margin-top: 20px;">
			<div class="span10 offset1">
				<div class="row-fluid" style="text-align: center">
					<asp:Label ID="Help_Label" runat="server" Text="请键入关键字搜索施工单位" CssClass="help-inline"></asp:Label>
				</div>
				<div class="row-fluid">
					<div class="span8 offset2">
						<asp:ListView ID="Team_ListView" runat="server" Visible="false">
							<ItemTemplate>
								<div class="row-fluid ">
									<div class="well">
										<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "~/TeamPage/TeamDetailsPage.aspx?id="+Eval("ID")%>'><%# Eval("TeamName") %></asp:HyperLink>
									</div>
								</div>
							</ItemTemplate>
						</asp:ListView>
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
