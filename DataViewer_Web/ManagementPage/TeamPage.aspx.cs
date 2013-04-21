using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.ManagementPage
{
	public partial class TeamPage : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void On_SearchButton_Click(object sender, EventArgs e)
		{
			List<Team> teams = Team.Get_ByFuzzyTeamName(TeamName_TextBox.Text);
			if (teams.Count == 0)
			{
				Team_ListView.Visible = false;
				Help_Label.Visible = true;
				Help_Label.Text = "施工单位不存在, 请重新键入关键字!";
			}
			else
			{
				Team_ListView.DataSource = teams;
				Team_ListView.Visible = true;
				Help_Label.Visible = false;
				Team_ListView.DataBind();
			}
		}
	}
}