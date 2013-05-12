<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Company_Master.Master" AutoEventWireup="true" CodeBehind="CompanyEditPage.aspx.cs" Inherits="DataViewer_Web.CompanyPage.CompanyEditPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<style type="text/css">
		.well > div > input {
			margin-bottom: 0px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="Back_HyperLink" runat="server">返回</asp:HyperLink>
	</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server" class="form-horizontal">
		<div class="row-fluid">
			<div class="offset2 span8 well well-large">
				<div class="row-fluid">
					<div class="span12 control-group">
						<label class="control-label" for='<%= CompanyName_TextBox.ClientID %>'>单位名称:</label>
						<div class="controls">
							<asp:TextBox ID="CompanyName_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12 control-group">
						<label class="control-label" for='<%= CompanyName_TextBox.ClientID %>'>法人代表:</label>
						<div class="controls">
							<asp:TextBox ID="LegalRepresentative_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12 control-group">
						<label class="control-label" for='<%= CompanyName_TextBox.ClientID %>'>单位地址:</label>
						<div class="controls">
							<asp:TextBox ID="Address_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid" style="text-align: center">
					<asp:Button ID="Submit_Button" runat="server" Text="保存" CssClass="btn" Width="100" OnClick="On_SubmitButton_Click" />
				</div>
			</div>
		</div>
	</form>
</asp:Content>
