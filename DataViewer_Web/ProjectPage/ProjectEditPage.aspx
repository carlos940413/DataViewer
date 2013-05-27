<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Project_Master.master" AutoEventWireup="true" CodeBehind="ProjectEditPage.aspx.cs" Inherits="DataViewer_Web.ProjectPage.ProjectEditPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="Back_HyperLink" runat="server">返回</asp:HyperLink>
	</li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPart" runat="server">
	<form id="form1" runat="server" class="form-horizontal">
		<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
		<div class="container">
			<div class="span8 offset1">
				<div class="row-fluid">
					<div class="row-fluid">
						<div class="control-group span12">
							<label class="control-label" for='<%= ProjectName_TextBox.ClientID %>'>项目名称: </label>
							<div class="controls">
								<asp:TextBox ID="ProjectName_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span6">
							<label class="control-label">经度</label>
							<div class="controls">
								<div class="row-fluid">
									<div class="input-append span4">
										<asp:TextBox ID="EastDegree_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">°</span>
									</div>
									<div class="input-append span4">
										<asp:TextBox ID="EastMinute_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">′</span>
									</div>
									<div class="input-append span4">
										<asp:TextBox ID="EastSecond_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">″</span>
									</div>
								</div>
							</div>
						</div>
						<div class="control-group span6">
							<label class="control-label">纬度</label>
							<div class="controls">
								<div class="row-fluid">
									<div class="input-append span4">
										<asp:TextBox ID="NorthDegree_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">°</span>
									</div>
									<div class="input-append span4">
										<asp:TextBox ID="NorthMinute_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">′</span>
									</div>
									<div class="input-append span4">
										<asp:TextBox ID="NorthSecond_TextBox" CssClass="span9" runat="server"></asp:TextBox>
										<span class="add-on">″</span>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span6">
							<label class="control-label" for="<%= StartOn_Calendar.ClientID %>">起始时间</label>
							<div class="controls">
								<asp:Calendar ID="StartOn_Calendar" runat="server"></asp:Calendar>
							</div>
						</div>
						<div class="control-group span6">
							<label class="control-label" for="<%= EndOn_Plan_Calendar.ClientID %>">结束时间</label>
							<div class="controls">
								<asp:Calendar ID="EndOn_Plan_Calendar" runat="server"></asp:Calendar>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span6">
							<label class="control-label" for='<%= Region_DropDownList.ClientID %>'>所属区域: </label>
							<div class="controls">
								<asp:DropDownList ID="Region_DropDownList" DataTextField="RegionName" DataValueField="ID" runat="server" CssClass="input-block-level"></asp:DropDownList>
							</div>
						</div>
						<div class="control-group span6">
							<label class="control-label" for='<%= Company_DropDownList.ClientID %>'>建设单位:</label>
							<div class="controls">
								<asp:DropDownList ID="Company_DropDownList" runat="server" DataTextField="CompanyName" DataValueField="ID" CssClass="input-block-level"></asp:DropDownList>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span6">
							<label class="control-label" for='<%= DutyOfficerName_TextBox.ClientID %>'>负责人: </label>
							<div class="controls">
								<asp:TextBox ID="DutyOfficerName_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
							</div>
						</div>
						<div class="control-group span6">
							<label class="control-label" for='<%= DutyOfficerPhoneNumber_TextBox.ClientID %>'>联系方式: </label>
							<div class="controls">
								<asp:TextBox ID="DutyOfficerPhoneNumber_TextBox" runat="server" CssClass="input-block-level"></asp:TextBox>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span12">
							<label class="control-label">施工单位</label>
							<div class="controls">
								<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<div class="row-fluid">
											<div class="well well-small span12">
												<div class="row-fluid">
													<div class="control-group span12">
														<label class="control-label">施工单位</label>
														<div class="controls">
															<asp:DropDownList ID="Team_DropDownList" DataTextField="TeamName" DataValueField="ID" CssClass="input-block-level" runat="server"></asp:DropDownList>
														</div>
													</div>
												</div>
												<div class="row-fluid">
													<div class="control-group span5">
														<label class="control-label">负责人</label>
														<div class="controls">
															<asp:TextBox ID="TeamDutyOfficer_Name" CssClass="input-block-level" runat="server"></asp:TextBox>
														</div>
													</div>
													<div class="control-group span5">
														<label class="control-label">联系方式</label>
														<div class="controls">
															<asp:TextBox ID="TeamDutyOfficer_PhoneNumber" CssClass="input-block-level" runat="server"></asp:TextBox>
														</div>
													</div>
													<div class="span2" style="text-align: center">
														<asp:Button ID="AddTeam_Button" runat="server" Text="添加" CssClass="btn" OnClick="On_AddTeamButton_Click" />
													</div>
												</div>
											</div>
										</div>
										<div class="row-fluid">
											<table class="table table-bordered">
												<tr>
													<th>施工工地</th>
													<th>负责人</th>
													<th>联系方式</th>
													<th></th>
												</tr>
												<asp:ListView ID="TeamInformation_ListView" runat="server" OnItemCommand="On_TeamInformationListView_ItemCommand">
													<ItemTemplate>
														<tr>
															<td><%# Eval("Key.TeamName") %></td>
															<td><%# Eval("Value.PersonName") %></td>
															<td><%# Eval("Value.PhoneNumber") %></td>
															<td>
																<asp:LinkButton runat="server" CommandArgument='<%# Container.DataItemIndex %>' CommandName="DeleteItem">删除</asp:LinkButton>
															</td>
														</tr>
													</ItemTemplate>
												</asp:ListView>
											</table>
										</div>
									</ContentTemplate>
								</asp:UpdatePanel>
							</div>
						</div>
					</div>
					<div class="row-fluid">
						<div class="control-group span12">
							<label class="control-label">监管单位</label>
							<div class="controls">
								<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<div class="row-fluid">
											<div class="span12 row-fluid well well-small">
												<div class="span10">
													<asp:DropDownList ID="SupervisionDepartment_DropDownList" runat="server" DataTextField="DepartmentName" DataValueField="ID" CssClass="input-block-level"></asp:DropDownList>
												</div>
												<div class="span2" style="text-align: center">
													<asp:Button ID="AddSupervisionDepartment_Button" runat="server" Text="添加" CssClass="btn" OnClick="On_AddSupervisionDepartmentButton_Click" />
												</div>
											</div>
										</div>
										<div class="row-fluid">
											<table class="table table-bordered">
												<tr>
													<th>监管单位名称</th>
													<th></th>
												</tr>
												<asp:ListView ID="SupervisionDepartment_ListView" runat="server" OnItemCommand="On_SupervisionDepartmentListView_ItemCommand">
													<ItemTemplate>
														<tr>
															<td><%# Eval("DepartmentName") %></td>
															<td>
																<asp:LinkButton runat="server" CommandArgument="<%# Container.DataItemIndex %>">删除</asp:LinkButton>
															</td>
														</tr>
													</ItemTemplate>
												</asp:ListView>
											</table>
										</div>
									</ContentTemplate>
								</asp:UpdatePanel>
							</div>
						</div>
					</div>
					<div class="row-fluid" style="text-align: center">
						<asp:Button ID="Submit_Button" runat="server" Text="保存" CssClass="btn" Width="100" OnClick="On_SubmitButton_Click" />
					</div>
				</div>
			</div>
		</div>
	</form>
</asp:Content>
