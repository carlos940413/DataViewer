<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="DataViewer_Web.Index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script type="text/javascript" src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <title></title>
    <style>
        #map {
            width: 600px;
            height: 600px;
        }
    </style>
    <script type="text/javascript">
        $().ready(function () {
            var map = new BMap.Map("map");
            var points = new Array();
            var projectnames=new Array();
            <%for (int i = 0; i < projects.Count; i++){%>
                points[<%=i%>]=new BMap.Point(<%=projects[i].Location_East%>, <%=projects[i].Location_North%>);
                projectnames[<%=i%>]="<%=projects[i].ProjectName%>";
            <%}%>
            if (points.length==0)
                map.centerAndZoom("南京");
            else
            {
                var viewport=map.getViewport(points);
                map.centerAndZoom(viewport.center,viewport.zoom);
                for (var i=0; i<points.length; i++)
                {
                    var marker=new BMap.Marker(points[i]);
                    marker.setTitle(projectnames[i]);
                    map.addOverlay(marker);
                }
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="map">
        </div>
    </form>
</body>
</html>
