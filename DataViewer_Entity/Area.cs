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

        /// <summary>
        /// 为某项目创建区域
        /// </summary>
        /// <param name="project">项目名称</param>
        /// <returns>返回新建的Area对象，地区ID为0，地区名称为空，项目为该项目。若该项目不存在，返回Null</returns>
		public static Area CreateArea(Project project)
		{
			if (project == null || project.ID == 0)
				return null;
			return new Area() { Project = project };
		}

        #region Properties
        /// <summary>
        /// 区域ID
        /// </summary>
        private int _ID;
		public int ID
		{
			get { return _ID; }
			set { _ID = value; }
		}

        /// <summary>
        /// 区域所属项目
        /// </summary>
		private Project _Project;
		public Project Project
		{
			get { return _Project; }
			set { _Project = value; }
		}

        /// <summary>
        /// 区域名称
        /// </summary>
		private string _AreaName;
		public string AreaName
		{
			get { return _AreaName; }
			set { _AreaName = value; }
		}
        #endregion

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

        /// <summary>
        /// 根据区域ID获取区域
        /// </summary>
        /// <param name="id">区域ID</param>
        /// <returns>返回待查找的区域。如果未找到, 返回Null</returns>
		public static Area Get_ByID(int id)
		{
			List<Area> areas = toList(DBHelper.SelectCommand("Area_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (areas.Count == 0)
				return null;
			return areas[0];
		}

        /// <summary>
        /// 根据项目ID获取该项目中的所有区域
        /// </summary>
        /// <param name="projectid">项目ID</param>
        /// <returns>若项目中没有区域，返回count=0的List</returns>
		public static List<Area> Get_ByProjectID(int projectid)
		{
			return toList(DBHelper.SelectCommand("Area_projectid", CommandType.StoredProcedure,
				new SqlParameter("@projectid", projectid)));
		}

        /// <summary>
        /// 获取所有的区域
        /// </summary>
        /// <returns></returns>
		public static List<Area> Get_All()
		{
			return toList(DBHelper.SelectCommand("Area_all", CommandType.StoredProcedure));
		}
	}
}
