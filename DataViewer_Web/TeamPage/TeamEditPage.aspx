<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Team_Master.master" AutoEventWireup="true" CodeBehind="TeamEditPage.aspx.cs" Inherits="DataViewer_Web.TeamPage.TeamEditPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="Back_HyperLink" runat="server">返回</asp:HyperLink>
	</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server" class="form-horizontal">
		<div class="row-fluid">
			<div class="span8 offset2 well well-large">
				<div class="row-fluid">
					<div class="control-group span12">
						<label class="control-label" for='<%= TeamName_TextBox.ClientID %>'>单位名称:</label>
						<div class="controls">
							<asp:TextBox ID="TeamName_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="control-group span12">
						<label class="control-label" for='<%= LegalRepresentative_TextBox.ClientID %>'>法人代表:</label>
						<div class="controls">
							<asp:TextBox ID="LegalRepresentative_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="control-group span12">
						<label class="control-label" for='<%= Address_TextBox.ClientID %>'>单位地址:</label>
						<div class="controls">
							<asp:TextBox ID="Address_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="control-group span12">
						<label class="control-label" for='<%= TeamLevel_DropDownList.ClientID %>'>资质等级:</label>
						<div class="controls">
							<asp:DropDownList ID="TeamLevel_DropDownList" DataTextField="LevelName" DataValueField="ID" runat="server" CssClass="input-block-level"></asp:DropDownList>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="control-group span12">
						<label class="control-label" for='<%= TeamType_DropDownList.ClientID %>'>分包类型:</label>
						<div class="controls">
							<asp:DropDownList ID="TeamType_DropDownList" DataTextField="TypeName" DataValueField="ID" runat="server" CssClass="input-block-level"></asp:DropDownList>
						</div>
					</div>
				</div>
				<div class="row-fluid" style="text-align: center">
					<asp:Button ID="Submit_Button" runat="server" Text="保存" CssClass="btn" OnClick="On_SubmitButton_Click" />
				</div>
			</div>
		</div>
	</form>
</asp:Content>
