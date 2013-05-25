using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;
using System.Text;
using System.Data;

namespace DataViewer_Web.ProjectPage
{
	public partial class ProjectDetailsPage : System.Web.UI.Page
	{
		private static int pageSize = 5;
		private static int pageButtonCount = 7;
		protected int currentViewID;

		protected class AreaView
		{
			private int _AreaID;
			public int AreaID
			{
				get { return _AreaID; }
				set { _AreaID = value; }
			}

			private List<int> _Pager;
			public List<int> Pager
			{
				get { return _Pager; }
				set { _Pager = value; }
			}

			private int _CurrentPage;
			public int CurrentPage
			{
				get { return _CurrentPage; }
				set { _CurrentPage = value; }
			}

			private int _PageCount;
			public int PageCount
			{
				get { return _PageCount; }
				set { _PageCount = value; }
			}

			private DataTable _Concentrations;
			public DataTable Concentrations
			{
				get { return _Concentrations; }
				set { _Concentrations = value; }
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			int id;
			Project project = null;
			if (Session["Administrator"] == null)
				ChangeProject_HyperLink.Visible = false;
			if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
				project = Project.Get_ByID(id);
			if (project != null)
			{
				if (!IsPostBack)
				{
					#region Initialize Map
					StringBuilder scriptString = new StringBuilder();
					scriptString.Append("$().ready(function(){");
					scriptString.Append("var map=new BMap.Map('map');");
					scriptString.AppendFormat("var point=new BMap.Point({0},{1});", project.Location_East, project.Location_North);
					scriptString.Append("var marker=new BMap.Marker(point);");
					scriptString.Append("map.addOverlay(marker);");
					scriptString.Append("map.centerAndZoom(point,15);");
					scriptString.Append("map.disableDragging()");
					scriptString.Append("});");
					if (!Page.ClientScript.IsClientScriptBlockRegistered("mapScript"))
						Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "mapScript", scriptString.ToString(), true);
					#endregion

					#region Initialize Project Information
					ProjectName_Label.Text = project.ProjectName;
					DutyOfficerName_Label.Text = project.DutyOfficer.PersonName;
					DutyOfficerPhoneNumber_Label.Text = project.DutyOfficer.PhoneNumber;
					CompanyName_HyperLink.Text = project.Company.CompanyName;
					CompanyName_HyperLink.NavigateUrl = "/CompanyPage/CompanyDetailsPage.aspx?id=" + project.Company.ID;
					Region_Label.Text = project.Region.RegionName;
					Time_Label.Text = string.Format("{0:yyyy}年{0:MM}月{0:dd}日 - {1:yyyy}年{1:MM}月{1:dd}日", project.StartOn, project.EndOn_Plan);
					TeamInformation_ListView.DataSource = project.TeamInformation;
					SupervisionDepartment_ListView.DataSource = project.SupervisionDepartments;
					#endregion

					ChangeProject_HyperLink.NavigateUrl = "/ProjectPage/ProjectEditPage.aspx?id=" + project.ID;

					// Initialize Concentrations
					List<Area> areas = Area.Get_ByProjectID(project.ID);
					if (areas.Count == 0)
					{
						Help_Label.Visible = true;
						Concentrations_ListView.Visible = false;
					}
					else
					{
						Help_Label.Visible = false;
						Concentrations_ListView.Visible = true;
						// Initialize Area Tabs
						Session["Areas"] = areas;
						Area_ListView.DataSource = areas;
						currentViewID = areas[0].ID;
						// Initialize Concentration Content For Each Tab
						List<AreaView> areaViews = new List<AreaView>();
						foreach (Area area in areas)
						{
							areaViews.Add(GenerateAreaView(area, 0));
						}
						Session["Concentrations"] = areaViews;
						Concentrations_ListView.DataSource = areaViews;
					}
					this.DataBind();
				}
			}
			else
				Response.Redirect("/ProjectPage/ProjectPage.aspx");
		}

		/// <summary>
		/// 为每个Area生成一个div用来表示其中的数据
		/// </summary>
		/// <param name="area"></param>
		/// <param name="page">从0开始</param>
		/// <returns></returns>
		private AreaView GenerateAreaView(Area area, int page)
		{
			DataTable dt = new DataTable();
			// Initialize Columns of DataTable
			List<Node> nodes = Node.Get_ByAreaID(area.ID);
			if (nodes.Count != 0)
			{
				Dictionary<int, int> nodeID_columnIndex = new Dictionary<int, int>();
				for (int i = 0; i < nodes.Count; i++)
				{
					dt.Columns.Add(new DataColumn("节点 " + nodes[i].HardwareID));
					nodeID_columnIndex.Add(nodes[i].ID, i);
				}
				dt.Columns.Add(new DataColumn("采集时间"));
				// Initialize DataTable
				List<DateTime> acquireOns = Concentration.GetAcquireOn_ByAreaIDANDStartTimeANDEndTime(area.ID, DateTime.MinValue, DateTime.MinValue);
				int pageCount = (int)Math.Ceiling(acquireOns.Count / pageSize * 1.0);
				int startIndex = Math.Min(page * pageSize, acquireOns.Count - 1);
				int endIndex = Math.Min((page + 1) * pageSize - 1, acquireOns.Count - 1);
				List<Concentration> concentrations = Concentration.Get_ByAreaIDANDStartTimeANDEndTime(area.ID, acquireOns[endIndex], acquireOns[startIndex]);
				List<Concentration>.Enumerator concentrationsEnumerator = concentrations.GetEnumerator();
				concentrationsEnumerator.MoveNext();
				for (int i = startIndex; i <= endIndex; i++)
				{
					DataRow row = dt.NewRow();
					while (concentrationsEnumerator.Current.AcquireOn == acquireOns[i])
					{
						if (concentrationsEnumerator.Current.Amount > 0)
							row[nodeID_columnIndex[concentrationsEnumerator.Current.Node.ID]] = concentrationsEnumerator.Current.Amount;
						else
							row[nodeID_columnIndex[concentrationsEnumerator.Current.Node.ID]] = "过高";
						if (!concentrationsEnumerator.MoveNext())
							break;
					}
					row[dt.Columns.Count - 1] = acquireOns[i];
					dt.Rows.Add(row);
				}
				// Initialize Pager List
				List<int> pager = new List<int>();
				if (page < Math.Ceiling(pageButtonCount / 2.0))
					for (int i = 0; i < pageButtonCount; i++)
						pager.Add(i + 1);
				else if (page >= (pageCount - pageButtonCount / 2))
					for (int i = pageCount - pageButtonCount; i < pageCount; i++)
						pager.Add(i + 1);
				else
				{
					int startPageNumber = Math.Max(0, page - (pageButtonCount - 1) / 2);
					int endPageNumber = Math.Min(pageCount, page + (pageButtonCount - (pageButtonCount - 1) / 2 - 1));
					for (int i = startPageNumber; i <= endPageNumber; i++)
						pager.Add(i + 1);
				}
				return new AreaView() { AreaID = area.ID, Concentrations = dt, CurrentPage = page, Pager = pager, PageCount = pageCount };
			}
			else
				return new AreaView() { AreaID = area.ID, Concentrations = new DataTable(), CurrentPage = page, Pager = new List<int>(), PageCount = 0 };
		}

		private void UpdateConcentrations(int page)
		{
			List<AreaView> areaViews = Session["Concentrations"] as List<AreaView>;
			if (areaViews != null)
			{
				for (int i = 0; i < areaViews.Count; i++)
				{
					if (areaViews[i].AreaID == currentViewID)
					{
						areaViews[i] = GenerateAreaView(Area.Get_ByID(currentViewID), page);
						break;
					}
				}
				Session["Concentrations"] = areaViews;
				Concentrations_ListView.DataSource = areaViews;
				Concentrations_ListView.DataBind();
			}
			Area_ListView.DataSource = Session["Areas"];
			Area_ListView.DataBind();
		}

		protected void On_PreviousLinkButton_Command(object sender, CommandEventArgs e)
		{
			string[] arguments = e.CommandArgument.ToString().Split(':');
			currentViewID = Int32.Parse(arguments[0]);
			int page = Int32.Parse(arguments[1]) - 1;
			UpdateConcentrations(page);
		}

		protected void On_NextLinkButton_Command(object sender, CommandEventArgs e)
		{
			string[] arguments = e.CommandArgument.ToString().Split(':');
			currentViewID = Int32.Parse(arguments[0]);
			int page = Int32.Parse(arguments[1]) + 1;
			UpdateConcentrations(page);
		}

		protected void On_PagerReapter_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			currentViewID = Int32.Parse(e.CommandArgument.ToString().Split(':')[0]);
			int page = Int32.Parse(e.CommandArgument.ToString().Split(':')[1]) - 1;
			UpdateConcentrations(page);
		}
	}
}