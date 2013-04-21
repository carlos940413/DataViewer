using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataViewer_Entity
{
	public class Area : IEntity
	{
		private Area()
		{
			ID = 0;
			AreaName = "";
		}

		public static Area CreateArea(Project project)
		{
			if (project == null || project.ID == 0)
				return null;
			return new Area() { Project = project };
		}

		private int _ID;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

		private Project _Project;
		public Project Project
		{
			get { return _Project; }
			set { _Project = value; }
		}

		private string _AreaName;
		public string AreaName
		{
			get { return _AreaName; }
			set { _AreaName = value; }
		}

		public void Save()
		{
			if (ID == 0)
			{
				ID = DBHelper.InsertCommand("Area_Insert", CommandType.StoredProcedure,
					new SqlParameter("@projectid", Project.ID),
					new SqlParameter("@areaname", AreaName));
			}
			else
			{
				DBHelper.UpdateDeleteCommand("Area_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@projectid", Project),
					new SqlParameter("@areaname", AreaName));
			}
		}

		private static List<Area> toList(DataTable dt)
		{
			List<Area> result = new List<Area>();
			foreach (DataRow row in dt.Rows)
			{
				Area area = new Area();
				area.ID = Int32.Parse(row["id"].ToString());
				area.Project = Project.Get_ByID(Int32.Parse(row["projectid"].ToString()));
				area.AreaName = row["areaname"].ToString();
				result.Add(area);
			}
			return result;
		}

		public static Area Get_ByID(int id)
		{
			List<Area> areas = toList(DBHelper.SelectCommand("Area_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (areas.Count == 0)
				return null;
			return areas[0];
		}

		public static List<Area> Get_ByProjectID(int projectid)
		{
			return toList(DBHelper.SelectCommand("Area_projectid", CommandType.StoredProcedure,
				new SqlParameter("@projectid", projectid)));
		}

		public static List<Area> Get_All()
		{
			return toList(DBHelper.SelectCommand("Area_all", CommandType.StoredProcedure));
		}
	}
}
