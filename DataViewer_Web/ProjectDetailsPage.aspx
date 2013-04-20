<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Home_Master.Master" AutoEventWireup="true" CodeBehind="ProjectDetailsPage.aspx.cs" Inherits="DataViewer_Web.ProjectDetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" runat="server">
	<ul class="nav">
		<li><a href="Index.aspx">首页</a></li>
		<li class="active"><a href="ProjectPage.aspx">工地</a></li>
	</ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form runat="server" class="container-fluid">
		<div class="row-fluid">
			<div class="row-fluid">
				<div class="span5 offset1 well well-small" aria-orientation="horizontal">
					<div class="span11 offset1">
						<div class="row-fluid" style="margin-top: 42px;">
							<asp:Label ID="ProjectName_Label" runat="server" Text="Label"></asp:Label>
						</div>
						<div class="row-fluid" style="margin-top: 20px;">所属企业</div>
						<div class="row-fluid" style="margin-top: 5px;">
							<asp:Label ID="CompanyName_Label" runat="server" Text="Label"></asp:Label>
						</div>
						<div class="row-fluid" style="margin-top: 20px;">施工单位</div>
						<div class="row-fluid" style="margin-top: 5px;">
							<asp:Label ID="TeamName_Label" runat="server" Text="Label"></asp:Label>
						</div>
						<div class="row-fluid" style="margin-top: 20px;">项目起止时间</div>
						<div class="row-fluid" style="margin-bottom: 42px; margin-top: 5px;">
							<asp:Label ID="Time_Label" runat="server" Text="Label"></asp:Label>
						</div>
					</div>
				</div>
				<div class="span5 well well-small" id="map" style="height: 320px;"></div>
			</div>
			<div class="row-fluid" style="text-align: center">
				<asp:Label ID="Help_Label" runat="server" Text="暂无浓度监测数据" CssClass="help-inline"></asp:Label>
			</div>
			<div class="row-fluid">
				<div class="offset2 span8">
					<asp:GridView ID="ConcentrationData_GridView" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered"></asp:GridView>
				</div>
			</div>
			<div class="row-fluid pagination pagination-centered">
				<ul>
					<li class='<%# Session["CurrentPage"].ToString()=="1"?"disabled":"" %>'>
						<asp:LinkButton ID="Previous_LinkButton" runat="server" OnClick="On_PreviousLinkButton_Click">&lt;&lt;</asp:LinkButton>
					</li>
					<asp:Repeater ID="Pager_Repeater" runat="server">
						<ItemTemplate>
							<li class='<%# String.Equals(Container.DataItem.ToString(),Session["CurrentPage"].ToString())?"active":"" %>'>
								<asp:LinkButton runat="server" CommandArgument="<%# Container.DataItem %>" OnCommand="On_PagerRepeaterItem_Command"><%# Container.DataItem %></asp:LinkButton>
							</li>
						</ItemTemplate>
					</asp:Repeater>
					<li class='<%# Session["CurrentPage"].ToString()==pageCount.ToString()?"disabled":"" %>'>
						<asp:LinkButton ID="Next_LinkButton" runat="server" OnClick="On_NextLinkButton_Click">&gt;&gt;</asp:LinkButton>
					</li>
				</ul>
			</div>
		</div>
	</form>
</asp:Content>
