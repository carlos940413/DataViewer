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
	<form id="form1" runat="server" class="container-fluid">
		<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		<div class="row-fluid">
			<div class="span7">
				<div class="row-fluid">
					<div class="span12">
						<asp:Label runat="server" Text="项目名称: "></asp:Label>
						<asp:Label ID="ProjectName_Label" runat="server"></asp:Label>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span6">
						<asp:Label runat="server" Text="项目负责人: "></asp:Label>
						<asp:Label ID="DutyOfficerName_Label" runat="server" Text="Label"></asp:Label>
					</div>
					<div class="span6">
						<asp:Label runat="server" Text="联系方式: "></asp:Label>
						<asp:Label ID="DutyOfficerPhoneNumber_Label" runat="server" Text="Label"></asp:Label>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12">
						<asp:Label runat="server" Text="建设单位: "></asp:Label>
						<asp:HyperLink ID="CompanyName_HyperLink" runat="server">HyperLink</asp:HyperLink>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12">
						<asp:Label ID="Label1" runat="server" Text="所属区域:"></asp:Label>
						<asp:Label ID="Region_Label" runat="server" Text="Label"></asp:Label>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12">
						<asp:Label runat="server" Text="项目起止时间: "></asp:Label>
						<asp:Label ID="Time_Label" runat="server" Text="Label"></asp:Label>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12">
						<div class="row-fluid">
							施工单位信息
						</div>
						<div class="row-fluid">
							<table class="table table-bordered">
								<tr>
									<th>施工单位</th>
									<th>负责人</th>
									<th>联系电话</th>
								</tr>
								<asp:ListView ID="TeamInformation_ListView" runat="server">
									<ItemTemplate>
										<tr>
											<td><a href='<%# "/TeamPage/TeamDetailsPage.aspx?id="+Eval("Key.ID") %>'><%# Eval("Key.TeamName") %></a></td>
											<td><%# Eval("Value.PersonName") %></td>
											<td><%# Eval("Value.PhoneNumber") %></td>
										</tr>
									</ItemTemplate>
								</asp:ListView>
							</table>
						</div>
					</div>
				</div>
				<div class="row-fluid">
					<div class="span12">
						<div class="row-fluid">
							监管单位信息
						</div>
						<div class="row-fluid">
							<table class="table table-bordered">
								<tr>
									<th>监管单位名称</th>
									<th>联系方式</th>
								</tr>
								<asp:ListView ID="SupervisionDepartment_ListView" runat="server">
									<ItemTemplate>
										<tr>
											<td><%# Eval("DepartmentName") %></td>
											<td><%# Eval("PhoneNumber") %></td>
										</tr>
									</ItemTemplate>
								</asp:ListView>
							</table>
						</div>
					</div>
				</div>
			</div>
			<div class="span5">
				<div class="row-fluid">
					<div id="map" class="well well-small" style="height: 300px;"></div>
				</div>
				<div class="row-fluid" style="text-align: center">
					<asp:Label ID="Help_Label" runat="server" Text="暂无浓度监测数据" CssClass="help-inline"></asp:Label>
				</div>
				<div class="row-fluid">
					<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<div class="tabbable">
								<ul class="nav nav-tabs">
									<asp:ListView ID="Area_ListView" runat="server">
										<ItemTemplate>
											<li class='<%# Int32.Parse(Eval("ID").ToString())==currentViewID?"active":"" %>'><a href='<%# "#"+Eval("ID") %>' data-toggle="tab"><%# Eval("AreaName") %></a></li>
										</ItemTemplate>
									</asp:ListView>
								</ul>
								<div class="tab-content">
									<asp:ListView ID="Concentrations_ListView" runat="server">
										<ItemTemplate>
											<div class='<%# Int32.Parse(Eval("AreaID").ToString())==currentViewID?"tab-pane active":"tab-pane" %>' id='<%# Eval("AreaID") %>'>
												<div class="row-fluid">
													<div class="span12">
														<asp:GridView runat="server" AutoGenerateColumns="true" CssClass="table table-bordered" DataSource='<%# Eval("Concentrations") %>'></asp:GridView>
													</div>
												</div>
												<div class="row-fluid pagination pagination-centered">
													<ul>
														<li class='<%# Eval("CurrentPage").ToString()=="0"?"disabled":"" %>'>
															<asp:LinkButton ID="Previous_LinkButton" runat="server"
																CommandArgument='<%# Eval("AreaID").ToString()+":"+Eval("CurrentPage").ToString() %>'
																Enabled='<%# Eval("CurrentPage").ToString()!="0" %>'
																Visible='<%# Eval("PageCount").ToString()!="0" %>'
																OnCommand="On_PreviousLinkButton_Command">&lt;&lt;</asp:LinkButton>
														</li>
														<asp:Repeater ID="Pager_Reapter" runat="server" DataSource='<%# Eval("Pager") %>' OnItemCommand="On_PagerReapter_ItemCommand">
															<ItemTemplate>
																<li class='<%# Int32.Parse(DataBinder.Eval(Container.Parent.Parent,"DataItem.CurrentPage").ToString())==Int32.Parse(Container.DataItem.ToString())-1?"active":"" %>'>
																	<asp:LinkButton runat="server"
																		CommandArgument='<%# DataBinder.Eval(Container.Parent.Parent, "DataItem.AreaID").ToString() +":"+Container.DataItem.ToString() %>'
																		Enabled='<%# Int32.Parse(DataBinder.Eval(Container.Parent.Parent,"DataItem.CurrentPage").ToString())!=Int32.Parse(Container.DataItem.ToString())-1 %>'><%# Container.DataItem %></asp:LinkButton>
																</li>
															</ItemTemplate>
														</asp:Repeater>
														<li class='<%# Int32.Parse(Eval("CurrentPage").ToString())==Int32.Parse(Eval("PageCount").ToString())-1?"disabled":"" %>'>
															<asp:LinkButton ID="Next_LinkButton" runat="server"
																CommandArgument='<%# Eval("AreaID").ToString()+":"+Eval("CurrentPage").ToString() %>'
																Enabled='<%# Int32.Parse(Eval("CurrentPage").ToString())!=Int32.Parse(Eval("PageCount").ToString())-1 %>'
																Visible='<%# Eval("PageCount").ToString()!="0" %>'
																OnCommand="On_NextLinkButton_Command">&gt;&gt;</asp:LinkButton>
														</li>
													</ul>
												</div>
											</div>
										</ItemTemplate>
									</asp:ListView>
								</div>
							</div>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
