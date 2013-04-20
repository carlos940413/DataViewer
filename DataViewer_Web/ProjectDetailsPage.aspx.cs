using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;
using System.Text;
using System.Data;

namespace DataViewer_Web
{
    public partial class ProjectDetailsPage : System.Web.UI.Page
    {
        private static int pageSize = 5;
        private static int pageButtonCount = 7;
        protected int pageCount;

        /// <summary>
        /// 生成浓度表格
        /// </summary>
        /// <param name="pageIndex">分页下标, 从0开始</param>
        private void GenerateConcentrationTable(int pageIndex)
        {
            DataTable concentrationData = new DataTable();
            Dictionary<int, int> nodeID_columnIndex = new Dictionary<int, int>();
            #region Generate Columns
            List<Node> nodes = Session["Nodes"] as List<Node>;
            for (int i = 0; i < nodes.Count; i++)
            {
                concentrationData.Columns.Add(new DataColumn("节点 " + nodes[i].ID));
                nodeID_columnIndex.Add(nodes[i].ID, i);
            }
            concentrationData.Columns.Add(new DataColumn("采集时间"));
            #endregion
            Project project = Session["Project"] as Project;
            List<DateTime> acquireTimes = Concentration.GetAcquireOn_ByProjectIDANDStartTimeANDEndTime(project.ID, DateTime.MinValue, DateTime.MinValue);
			if (acquireTimes.Count != 0)
			{
				pageCount = (int)Math.Ceiling(((double)acquireTimes.Count) / pageSize);
				#region Generate Rows
				List<Concentration> concentrations = Concentration.Get_ByProjectIDANDStartTimeANDEndTime(project.ID, acquireTimes[Math.Min(pageSize * pageIndex + pageSize - 1, acquireTimes.Count - 1)], acquireTimes[pageSize * pageIndex]);
				List<Concentration>.Enumerator concentrationEnumerator = concentrations.GetEnumerator();
				concentrationEnumerator.MoveNext();
				foreach (DateTime acquireTime in acquireTimes.GetRange(pageIndex * pageSize, Math.Min(pageSize, acquireTimes.Count - pageIndex * pageSize)))
				{
					DataRow row = concentrationData.NewRow();
					row["采集时间"] = acquireTime;
					do
					{
						if (concentrationEnumerator.Current.AcquireOn == acquireTime)
							row[nodeID_columnIndex[concentrationEnumerator.Current.Node.ID]] = concentrationEnumerator.Current.Amount.ToString()+" mg/L";
						else
							break;
					} while (concentrationEnumerator.MoveNext());
					concentrationData.Rows.Add(row);
				}
				#endregion
				ConcentrationData_GridView.DataSource = concentrationData;
				Pager_Repeater.DataSource = GeneratePageNumber(pageCount, pageIndex);
				this.DataBind();
				if (pageIndex == 0)
					Previous_LinkButton.Enabled = false;
				else
					Previous_LinkButton.Enabled = true;
				if (pageIndex == pageCount - 1)
					Next_LinkButton.Enabled = false;
				else
					Next_LinkButton.Enabled = true;
				// change control visible
				Help_Label.Visible = false;
				ConcentrationData_GridView.Visible = true;
				Pager_Repeater.Visible = true;
				Previous_LinkButton.Visible = true;
				Next_LinkButton.Visible = true;
			}
			else
			{
				Help_Label.Visible = true;
				ConcentrationData_GridView.Visible = false;
				Pager_Repeater.Visible = false;
				Previous_LinkButton.Visible = false;
				Next_LinkButton.Visible = false;
			}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int id;
            Project project;
            // Check if request url is valid
            if (Request.Params["id"] != null && Int32.TryParse(Request.Params["id"].ToString(), out id))
            {
                // Check if project is initialed, is changed
                if (Session["Project"] == null || (Session["Project"] as Project).ID != id)
                {
                    project = Project.Get_ByID(id);
                    // Check if the projectid is valid
                    if (project.ID == 0)
                    {
                        Response.Redirect("ProjectPage.aspx");
                        return;
                    }
                    else
                    {
                        Session["Project"] = project;
                        Session["Nodes"] = Node.Get_ByAreaID((Session["Project"] as Project).ID);
                    }
                }
                else
                    project = Session["Project"] as Project;
            }
            else
            {
                Response.Redirect("ProjectPage.aspx");
                return;
            }

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

            if (!IsPostBack)
            {
                #region Initialize Project Information
                ProjectName_Label.Text = project.ProjectName;
                CompanyName_Label.Text = project.Company.CompanyName;
                TeamName_Label.Text = project.Team.TeamName;
                Time_Label.Text = string.Format("{0:yyyy}年{0:MM}月{0:dd}日 - {1:yyyy}年{1:MM}月{1:dd}日", project.StartOn, project.EndOn_Plan);
                #endregion

                Session["CurrentPage"] = 1;
                GenerateConcentrationTable(0);
            }
        }

        protected void On_PagerRepeaterItem_Command(object sender, CommandEventArgs e)
        {
            int pageIndex = Int32.Parse(e.CommandArgument.ToString()) - 1;
            Session["CurrentPage"] = pageIndex + 1;
            GenerateConcentrationTable(pageIndex);
        }

        private List<int> GeneratePageNumber(int pageCount, int pageIndex)
        {
            List<int> result = new List<int>();
            if (pageCount <= pageButtonCount)
                for (int i = 0; i < pageCount; i++)
                    result.Add(i + 1);
            else
            {
                int startIndex = Math.Max(pageIndex - (int)Math.Floor(((double)pageButtonCount - 1) / 2), 0);
                int endIndex = Math.Min(pageIndex + (int)Math.Ceiling(((double)pageButtonCount - 1) / 2), pageCount - 1);
                if (endIndex - startIndex + 1 < pageButtonCount)
                {
                    if (startIndex == 0)
                        endIndex = pageButtonCount - 1;
                    else
                        startIndex = pageCount - pageButtonCount;
                }
                for (int i = startIndex; i <= endIndex; i++)
                    result.Add(i + 1);
            }
            return result;
        }

        protected void On_PreviousLinkButton_Click(object sender, EventArgs e)
        {
            int pageIndex = Int32.Parse(Session["CurrentPage"].ToString()) - 2;
            Session["CurrentPage"] = pageIndex + 1;
            GenerateConcentrationTable(pageIndex);
        }

        protected void On_NextLinkButton_Click(object sender, EventArgs e)
        {
            int pageIndex = Int32.Parse(Session["CurrentPage"].ToString());
            Session["CurrentPage"] = pageIndex + 1;
            GenerateConcentrationTable(pageIndex);
        }
    }
}