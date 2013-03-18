using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web
{
    public partial class Index : System.Web.UI.Page
    {
        public List<Project> projects;
        protected void Page_Load(object sender, EventArgs e)
        {
            projects = Project.Get_All();
            //string scriptForMap = "$().ready(function(){";
            //scriptForMap += "var map=new BMap.Map('map');";
            //if (projects.Count != 0)
            //{
            //    scriptForMap += "var points=new Array();";
            //    for (int i = 0; i < projects.Count; i++)
            //    {
            //        scriptForMap += string.Format("points[{0}]=new BMap.Point({1},{2})", i, projects[i].Location_East, projects[i].Location_North);
            //    }
            //    scriptForMap += "var viewport=map.GetViewport(points);";
            //    scriptForMap += "map.centerAndZoom(view.center,view.zoom);";
            //}
            //else
            //    scriptForMap += "map.centerAndZoom('南京');";
            //scriptForMap += "});";
            //if (!Page.ClientScript.IsClientScriptBlockRegistered("mapScript"))
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "mapScript", scriptForMap, true);
        }
    }
}