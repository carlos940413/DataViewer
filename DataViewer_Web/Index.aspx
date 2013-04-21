<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Home_Master.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DataViewer_Web.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <style>
        #map {
            height: 600px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavContent" runat="server">
	<ul class="nav">
		<li class="active"><a href="/Index.aspx">首页</a></li>
		<li><a>工程项目</a></li>
		<li><a href="/CompanyPage/CompanyPage.aspx">建设单位</a></li>
		<li><a href="/TeamPage/TeamPage.aspx">施工单位</a></li>
	</ul>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPart" runat="server">
    <form runat="server">
        <div class="row-fluid">
            <div class="span7 well well-small" id="map"></div>
            <div class="span5 well well-large">
                扬尘污染指数
                        <div id="concentration_details"></div>
            </div>
        </div>
    </form>
</asp:Content>
