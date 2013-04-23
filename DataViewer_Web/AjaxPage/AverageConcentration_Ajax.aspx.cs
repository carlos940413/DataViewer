using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataViewer_Entity;

namespace DataViewer_Web.AjaxPage
{
	public partial class AverageConcentration_Ajax : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			int projectID;
			if (Request.Params["projectid"] != null && Int32.TryParse(Request.Params["projectid"].ToString(), out projectID))
			{
				List<Area> areas = Area.Get_ByProjectID(projectID);
				Area_ListView.DataSource = areas;
				Dictionary<int, Dictionary<DateTime, double>> latestConcentrations = new Dictionary<int, Dictionary<DateTime, double>>();
				foreach (Area area in areas)
				{
					Dictionary<DateTime, double> averageConcentration = Concentration.GetAverage_ByAreaID(area.ID);
					Dictionary<DateTime, double>.KeyCollection keys = averageConcentration.Keys;
					Dictionary<DateTime, double> latestConcentration = new Dictionary<DateTime, double>();
					int count = 0;
					foreach (var key in keys)
					{
						latestConcentration.Add(key, averageConcentration[key]);
						if (++count == 10)
							break;
					}
					latestConcentrations.Add(area.ID, latestConcentration);
				}
				Concentrations_ListView.DataSource = latestConcentrations;
				this.DataBind();
			}
		}
	}
}