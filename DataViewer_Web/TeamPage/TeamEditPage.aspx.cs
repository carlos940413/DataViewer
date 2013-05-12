using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.TeamPage
{
	public partial class TeamEditPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int id;
				Team team = null;
				TeamLevel_DropDownList.DataSource = TeamLevel.Get_All();
				TeamType_DropDownList.DataSource = TeamType.Get_All();
				this.DataBind();
				if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
					team = Team.Get_ByID(id);
				if (team != null)
				{
					Back_HyperLink.NavigateUrl = "/TeamPage/TeamDetailsPage.aspx?id=" + team.ID;

					TeamName_TextBox.Text = team.TeamName;
					LegalRepresentative_TextBox.Text = team.LegalRepresentative;
					Address_TextBox.Text = team.Address;
					TeamLevel_DropDownList.SelectedValue = team.TeamLevel.ID.ToString();
					TeamType_DropDownList.SelectedValue = team.TeamType.ID.ToString();
				}
				else
				{
					Back_HyperLink.NavigateUrl = "/TeamPage/TeamPage.aspx";
				}
			}
		}

		protected void On_SubmitButton_Click(object sender, EventArgs e)
		{
			int id;
			Team team = null;
			if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
				team = Team.Get_ByID(id);
			if (team != null)
			{
				team.TeamLevel = TeamLevel.Get_ByID(Int32.Parse(TeamLevel_DropDownList.SelectedValue));
				team.TeamType = TeamType.Get_ByID(Int32.Parse(TeamType_DropDownList.SelectedValue));
			}
			else
			{
				team = Team.CreateTeam(TeamLevel.Get_ByID(Int32.Parse(TeamLevel_DropDownList.SelectedValue)), TeamType.Get_ByID(Int32.Parse(TeamType_DropDownList.SelectedValue)));
			}
			team.TeamName = TeamName_TextBox.Text;
			team.LegalRepresentative = LegalRepresentative_TextBox.Text;
			team.Address = Address_TextBox.Text;
			team.Save();
			Response.Redirect("/TeamPage/TeamDetailsPage.aspx?id=" + team.ID);
		}
	}
}