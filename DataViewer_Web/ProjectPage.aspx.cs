using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web
{
    public partial class ProjectPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void On_SearchButton_Click(object sender, EventArgs e)
        {
            List<Project> projects = Project.Get_ByFuzzyProjectName(ProjectName_TextBox.Text);
            if (projects.Count == 0)
            {
                Help_Label.Text = "施工工地未找到, 请更换关键字";
                Help_Label.Visible = true;
                Projects_ListView.Visible = false;
            }
            else
            {
                Help_Label.Visible = false;
                Projects_ListView.Visible = true;
                Projects_ListView.DataSource = projects;
                this.DataBind();
            }
        }
    }
}