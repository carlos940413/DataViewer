<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Home_Master.Master" AutoEventWireup="true" CodeBehind="ProjectPage.aspx.cs" Inherits="DataViewer_Web.ProjectPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btn {
            margin-top: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" runat="server">
    <ul class="nav">
        <li><a href="/Index.aspx">首页</a></li>
        <li class="active"><a href="ProjectPage.aspx">工地</a></li>
    </ul>
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
                                <asp:HyperLink ID="ProjectName_LinkButton" NavigateUrl='<%# "ProjectDetailsPage.aspx?id="+Eval("ID") %>' runat="server" Text='<%# Eval("ProjectName") %>' CssClass="span12"></asp:HyperLink>
                            </div>
                            <div class="row-fluid">
                                <div class="span6">
                                    所属企业:
                                    <asp:Label ID="Label1" runat="server" Text='<%#Eval("Company.CompanyName") %>'></asp:Label>
                                </div>
                                <div class="span6">
                                    施工单位:
                                    <asp:Label ID="Label2" runat="server" Text='<%#Eval("Team.TeamName") %>'></asp:Label>
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
