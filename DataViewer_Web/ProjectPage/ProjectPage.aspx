<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Project_Master.Master" AutoEventWireup="true" CodeBehind="ProjectPage.aspx.cs" Inherits="DataViewer_Web.ProjectPage.ProjectPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btn {
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Operation" runat="server">
	<li>
		<asp:HyperLink ID="CreateProject_HyperLink" runat="server" NavigateUrl="/ProjectPage/ProjectEditPage.aspx">添加工程项目</asp:HyperLink>
	</li>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPart" runat="server">
    <form runat="server" class="form-search">
        <div class="row-fluid">
            <div class="input-append row-fluid span8 offset2">
                <asp:TextBox ID="ProjectName_TextBox" runat="server" CssClass="search-query span10"></asp:TextBox>
                <asp:Button ID="Search_Button" runat="server" Text="搜索" CssClass="btn span2" OnClick="On_SearchButton_Click" />
            </div>
        </div>
        <div class="row-fluid" style="margin-top: 20px;">
            <div class="span10 offset1 well row-fluid">
                <div class="row-fluid" style="text-align: center">
                    <asp:Label ID="Help_Label" runat="server" Text="请键入关键字搜索施工工地" CssClass="help-inline"></asp:Label>
                </div>
                <asp:ListView ID="Projects_ListView" runat="server">
                    <ItemTemplate>
                        <div class="row-fluid">
                            <div class="row-fluid">
                                <asp:HyperLink ID="ProjectName_LinkButton" NavigateUrl='<%# "/ProjectPage/ProjectDetailsPage.aspx?id="+Eval("ID") %>' runat="server" Text='<%# Eval("ProjectName") %>' CssClass="span12"></asp:HyperLink>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    所属企业:
                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("Company.CompanyName") %>'></asp:Label>
                                </div>
                            </div>
                            <hr width="100%" />
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</asp:Content>
