using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.AjaxPage
{
    public partial class ProjectDetails_Ajax : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int projectID;
            bool hasID = Int32.TryParse(Request.Params["projectid"].ToString(), out projectID);
            if (hasID)
            {
                Project project = Project.Get_ByID(projectID);
                Company_Label.Text = project.Company.CompanyName;
                Period_Label.Text = string.Format("{0} - {1}", project.StartOn.ToShortDateString(), project.EndOn_Plan.ToShortDateString());
                Details_HyperLink.NavigateUrl = "/ProjectPage/ProjectDetailsPage.aspx?id=" + projectID;
            }
        }
    }
}