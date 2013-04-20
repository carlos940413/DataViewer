using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;
using System.Text;

namespace DataViewer_Web
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Project> projects = Project.Get_All();
            StringBuilder scriptString = new StringBuilder();
            scriptString.Append("$().ready(function(){");
            scriptString.Append("var map=new BMap.Map('map');");
            if (projects.Count != 0)
            {
                scriptString.Append("var points=new Array();");
                scriptString.Append("var markers=new Array();");
                for (int i = 0; i < projects.Count; i++)
                {
                    scriptString.AppendFormat("points[{0}]=new BMap.Point({1},{2});", i, projects[i].Location_East, projects[i].Location_North);
                    scriptString.AppendFormat("markers[{0}]=new BMap.Marker(points[{0}]);", i);
                    scriptString.AppendFormat("markers[{0}].setTitle('{1}');", i, projects[i].ProjectName);
                    scriptString.AppendFormat("markers[{0}].addEventListener('click',function(){{", i);
                    scriptString.AppendFormat("$.get('/AjaxPage/ProjectDetails_Ajax.aspx',{{projectid:{0}}},function(data,textStatus){{", projects[i].ID);
                    scriptString.Append("var infoWindow=new BMap.InfoWindow(data);");
                    scriptString.Append("infoWindow.addEventListener('clickclose',function(){");
                    scriptString.Append("$('#concentration_details').empty();");
                    scriptString.Append("});");
                    scriptString.AppendFormat("markers[{0}].openInfoWindow(infoWindow);", i);
                    scriptString.AppendFormat("$('#concentration_details').load('/AjaxPage/AverageConcentration_Ajax.aspx #details_content',{{projectid:{0}}});", projects[i].ID);
                    scriptString.Append("});");
                    scriptString.Append("});");
                    scriptString.AppendFormat("map.addOverlay(markers[{0}]);", i);
                }
                scriptString.Append("var viewport=map.getViewport(points);");
                scriptString.Append("map.centerAndZoom(viewport.center,viewport.zoom);");
                scriptString.Append("map.addControl(new BMap.NavigationControl());");
                scriptString.Append("map.enableScrollWheelZoom();");
            }
            else
                scriptString.Append("map.centerAndZoom('南京');");
            scriptString.Append("});");
            if (!Page.ClientScript.IsClientScriptBlockRegistered("mapScript"))
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "mapScript", scriptString.ToString(), true);
        }
    }
}