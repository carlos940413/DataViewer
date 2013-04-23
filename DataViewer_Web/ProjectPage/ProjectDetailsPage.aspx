<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Project_Master.Master" AutoEventWireup="true" CodeBehind="ProjectDetailsPage.aspx.cs" Inherits="DataViewer_Web.ProjectPage.ProjectDetailsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="ChangeProject_HyperLink" runat="server">修改工程项目</asp:HyperLink>
	</li>
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
				<div class="span10 tabbable offset1">
					<ul class="nav nav-tabs">
						<asp:ListView ID="Area_ListView" runat="server">
							<ItemTemplate>
								<li class='<%# Int32.Parse(Eval("ID").ToString())==currentView?"active":"" %>'><a href='<%# "#"+Eval("ID") %>' data-toggle="tab"><%# Eval("AreaName") %></a></li>
							</ItemTemplate>
						</asp:ListView>
					</ul>
					<div class="tab-content">
						<asp:ListView ID="Concentrations_ListView" runat="server">
							<ItemTemplate>
								<div class='<%# Int32.Parse(Eval("AreaID").ToString())==currentView?"tab-pane active":"tab-pane" %>' id='<%# Eval("AreaID") %>'>
									<div class="row-fluid">
										<div class="offset2 span8">
											<asp:GridView runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" DataSource='<%# Eval("Concentrations") %>'></asp:GridView>
										</div>
									</div>
									<div class="row-fluid pagination pagination-centered">
										<ul>
											<li class='<%# Eval("CurrentPage").ToString()=="0"?"disabled":"" %>'>
												<asp:LinkButton ID="Previous_LinkButton" runat="server" OnClick="On_PreviousLinkButton_Click">&lt;&lt;</asp:LinkButton>
											</li>
											<asp:Repeater runat="server" DataSource='<%# Eval("Pager") %>'>
												<ItemTemplate>
													<li class='<%# Int32.Parse(DataBinder.Eval(Container.Parent.Parent,"DataItem.CurrentPage").ToString())==Int32.Parse(Container.DataItem.ToString())-1?"active":"" %>'>
														<asp:LinkButton runat="server" CommandArgument='<%# DataBinder.Eval(Container.Parent.Parent, "DataItem.AreaID").ToString() +":"+Container.DataItem.ToString() %>' OnCommand="On_PagerRepeaterItem_Command"><%# Container.DataItem %></asp:LinkButton>
													</li>
												</ItemTemplate>
											</asp:Repeater>
											<li class='<%# Int32.Parse(Eval("CurrentPage").ToString())==Int32.Parse(Eval("PageCount").ToString())-1?"disabled":"" %>'>
												<asp:LinkButton ID="Next_LinkButton" runat="server" OnClick="On_NextLinkButton_Click">&gt;&gt;</asp:LinkButton>
											</li>
										</ul>
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
