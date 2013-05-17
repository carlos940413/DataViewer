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
		protected void Page_Load(object sender, EventArgs e)
		{
			int id;
			Project project = null;
			if (!IsPostBack)
			{
				Company_DropDownList.DataSource = Company.Get_All();
				Company_DropDownList.DataBind();
				Team_DropDownList.DataSource = Team.Get_All();
				Team_DropDownList.DataBind();
				Region_DropDownList.DataSource = Region.Get_All();
				Region_DropDownList.DataBind();
				SupervisionDepartment_DropDownList.DataSource = SupervisionDepartment.Get_All();
				SupervisionDepartment_DropDownList.DataBind();

				if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
					project = Project.Get_ByID(id);
				if (project != null)
				{
					Session["Project"] = project;
					Back_HyperLink.NavigateUrl = string.Format("/ProjectPage/ProjectDetailsPage.aspx?id={0}", project.ID);

					ProjectName_TextBox.Text = project.ProjectName;

					EastDegree_TextBox.Text = Math.Truncate(project.Location_East).ToString();
					double remain = project.Location_East - Math.Truncate(project.Location_East);
					EastMinute_TextBox.Text = Math.Truncate(remain * 60).ToString();
					remain = remain * 60 - Math.Truncate(remain * 60);
					EastSecond_TextBox.Text = Math.Truncate(remain * 60).ToString();

					NorthDegree_TextBox.Text = Math.Truncate(project.Location_North).ToString();
					remain = project.Location_North - Math.Truncate(project.Location_North);
					NorthMinute_TextBox.Text = Math.Truncate(remain * 60).ToString();
					remain = remain * 60 - Math.Truncate(remain * 60);
					NorthSecond_TextBox.Text = Math.Truncate(remain * 60).ToString();

					Region_DropDownList.SelectedValue = project.Region.ID.ToString();
					Company_DropDownList.SelectedValue = project.Company.ID.ToString();
					DutyOfficerName_TextBox.Text = project.DutyOfficer.PersonName;
					DutyOfficerPhoneNumber_TextBox.Text = project.DutyOfficer.PhoneNumber;
					TeamInformation_ListView.DataSource = project.TeamInformation;
					TeamInformation_ListView.DataBind();
					SupervisionDepartment_ListView.DataSource = project.SupervisionDepartments;
					SupervisionDepartment_ListView.DataBind();
				}
				else
				{
					Session["Project"] = null;
					Session["TeamInformation"] = new Dictionary<Team, DutyOfficer>();
					Session["SupervisionDepartments"] = new List<SupervisionDepartment>();
					Back_HyperLink.NavigateUrl = "/ProjectPage/ProjectPage.aspx";
				}
			}
		}

		protected void On_TeamInformationListView_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			Dictionary<Team, DutyOfficer> teamInformation;
			if (Session["Project"] == null)
			{
				teamInformation = Session["TeamInformation"] as Dictionary<Team, DutyOfficer>;
				teamInformation.Remove(teamInformation.Keys.ElementAt<Team>(Int32.Parse(e.CommandArgument.ToString())));
				Session["TeamInformation"] = teamInformation;
			}
			else
			{
				teamInformation = (Session["Project"] as Project).TeamInformation;
				teamInformation.Remove(teamInformation.Keys.ElementAt<Team>(Int32.Parse(e.CommandArgument.ToString())));
				(Session["Project"] as Project).TeamInformation = teamInformation;
			}
			TeamInformation_ListView.DataSource = teamInformation;
			TeamInformation_ListView.DataBind();
		}

		protected void On_AddTeamButton_Click(object sender, EventArgs e)
		{
			Dictionary<Team, DutyOfficer> teamInformation;
			if (Session["Project"] == null)
			{
				teamInformation = Session["TeamInformation"] as Dictionary<Team, DutyOfficer>;
				teamInformation.Add(Team.Get_ByID(Int32.Parse(Team_DropDownList.SelectedValue)), new DutyOfficer() { PersonName = TeamDutyOfficer_Name.Text, PhoneNumber = TeamDutyOfficer_PhoneNumber.Text });
				Session["TeamInformation"] = teamInformation;
			}
			else
			{
				teamInformation = (Session["Project"] as Project).TeamInformation;
				teamInformation.Add(Team.Get_ByID(Int32.Parse(Team_DropDownList.SelectedValue)), new DutyOfficer() { PersonName = TeamDutyOfficer_Name.Text, PhoneNumber = TeamDutyOfficer_PhoneNumber.Text });
				(Session["Project"] as Project).TeamInformation = teamInformation;
			}
			TeamInformation_ListView.DataSource = teamInformation;
			TeamInformation_ListView.DataBind();
			Team_DropDownList.SelectedIndex = 0;
			TeamDutyOfficer_Name.Text = "";
			TeamDutyOfficer_PhoneNumber.Text = "";
		}

		protected void On_AddSupervisionDepartmentButton_Click(object sender, EventArgs e)
		{
			List<SupervisionDepartment> supervisionDepartments;
			if (Session["Project"] == null)
			{
				supervisionDepartments = Session["SupervisionDepartment"] as List<SupervisionDepartment>;
				supervisionDepartments.Add(SupervisionDepartment.Get_ByID(Int32.Parse(SupervisionDepartment_DropDownList.SelectedValue)));
				Session["SupervisionDepartment"] = supervisionDepartments;
			}
			else
			{
				supervisionDepartments = (Session["Project"] as Project).SupervisionDepartments;
				supervisionDepartments.Add(SupervisionDepartment.Get_ByID(Int32.Parse(SupervisionDepartment_DropDownList.SelectedValue)));
				(Session["Project"] as Project).SupervisionDepartments = supervisionDepartments;
			}
			SupervisionDepartment_ListView.DataSource = supervisionDepartments;
			SupervisionDepartment_ListView.DataBind();
		}

		protected void On_SupervisionDepartmentListView_ItemCommand(object sender, ListViewCommandEventArgs e)
		{
			List<SupervisionDepartment> supervisionDepartments;
			if (Session["Project"] == null)
			{
				supervisionDepartments = Session["SupervisionDepartment"] as List<SupervisionDepartment>;
				supervisionDepartments.RemoveAt(Int32.Parse(e.CommandArgument.ToString()));
				Session["SupervisionDepartment"] = supervisionDepartments;
			}
			else
			{
				supervisionDepartments = (Session["Project"] as Project).SupervisionDepartments;
				supervisionDepartments.RemoveAt(Int32.Parse(e.CommandArgument.ToString()));
				(Session["Project"] as Project).SupervisionDepartments = supervisionDepartments;
			}
			SupervisionDepartment_ListView.DataSource = supervisionDepartments;
			SupervisionDepartment_ListView.DataBind();
		}

		protected void On_SubmitButton_Click(object sender, EventArgs e)
		{
			if (Session["Project"] == null)
			{
				Project project = Project.CreateProject(Company.Get_ByID(Int32.Parse(Company_DropDownList.SelectedValue)),
					Region.Get_ByID(Int32.Parse(Region_DropDownList.SelectedValue)),
					Session["TeamInformation"] as Dictionary<Team, DutyOfficer>,
					Session["SupervisionDepartment"] as List<SupervisionDepartment>);
				if (project != null)
				{
					project.ProjectName = ProjectName_TextBox.Text;
					project.DutyOfficer = new DutyOfficer() { PersonName = DutyOfficerName_TextBox.Text, PhoneNumber = DutyOfficerPhoneNumber_TextBox.Text };
					project.Location_East = Double.Parse(EastDegree_TextBox.Text) + Double.Parse(EastMinute_TextBox.Text) / 60 + Double.Parse(EastSecond_TextBox.Text) / 3600;
					project.Location_North = Double.Parse(NorthDegree_TextBox.Text) + Double.Parse(NorthMinute_TextBox.Text) / 60 + Double.Parse(NorthSecond_TextBox.Text) / 3600;
					project.Save();
					Response.Redirect("/ProjectPage/ProjectDetailsPage.aspx?id=" + project.ID);
				}
			}
			else
			{
				Project project = Session["Project"] as Project;
				project.ProjectName = ProjectName_TextBox.Text;
				project.Location_East = Double.Parse(EastDegree_TextBox.Text) + Double.Parse(EastMinute_TextBox.Text) / 60 + Double.Parse(EastSecond_TextBox.Text) / 3600;
				project.Location_North = Double.Parse(NorthDegree_TextBox.Text) + Double.Parse(NorthMinute_TextBox.Text) / 60 + Double.Parse(NorthSecond_TextBox.Text) / 3600;
				project.Region = Region.Get_ByID(Int32.Parse(Region_DropDownList.SelectedValue));
				project.Company = Company.Get_ByID(Int32.Parse(Company_DropDownList.SelectedValue));
				project.DutyOfficer = new DutyOfficer() { PersonName = DutyOfficerName_TextBox.Text, PhoneNumber = DutyOfficerPhoneNumber_TextBox.Text };
				project.Save();
				Response.Redirect("/ProjectPage/ProjectDetailsPage.aspx?id=" + project.ID);
			}
		}
	}
}