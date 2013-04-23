using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.ProjectPage
{
	public partial class ProjectEditPage : System.Web.UI.Page
	{
		protected Project project = null;
		protected void Page_Load(object sender, EventArgs e)
		{
			int id;
			if (!IsPostBack)
			{
				Company_DropDownList.DataSource = Company.Get_All();
				Team_DropDownList.DataSource = Team.Get_All();
			}
			if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
				project = Project.Get_ByID(id);
			if (project != null)
			{
				ProjectName_TextBox.Text = project.ProjectName;
				Company_DropDownList.SelectedValue = project.Company.ID.ToString();
				Team_DropDownList.SelectedValue = project.Team.ID.ToString();
			}
			this.DataBind();
		}
	}
}