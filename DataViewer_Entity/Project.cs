using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	public class Project : IEntity
	{
		private Project()
		{
			_ID = 0;
			ProjectName = "";
			DutyOfficer = new DutyOfficer();
			StartOn = DateTime.MinValue;
			EndOn_Plan = DateTime.MinValue;
			EndOn_Fact = DateTime.MinValue;
			Location_East = 0;
			Location_North = 0;
			Node_Phone = "";
		}

		public static Project CreateProject(Company company, Region region, Dictionary<Team, DutyOfficer> teamInformation, List<SupervisionDepartment> supervisionDepartments)
		{
			if (company == null || region == null || company.ID == 0 || region.ID == 0)
				return null;
			if (teamInformation == null || teamInformation.Keys.Count == 0)
				return null;
			bool hasIllegal = false;
			foreach (Team key in teamInformation.Keys)
			{
				if (key == null || key.ID == 0)
				{
					hasIllegal = true;
					break;
				}
			}
			if (hasIllegal)
				return null;
			if (supervisionDepartments == null || supervisionDepartments.Count == 0)
				return null;
			hasIllegal = false;
			foreach (SupervisionDepartment supervisionDepartment in supervisionDepartments)
			{
				if (supervisionDepartment.ID == 0)
				{
					hasIllegal = true;
					break;
				}
			}
			if (hasIllegal)
				return null;
			return new Project() { Company = company, Region = region, TeamInformation = teamInformation, SupervisionDepartments = supervisionDepartments };
		}

		#region Properties
		private int _ID;
		public int ID
		{
			get { return _ID; }
		}

		/// <summary>
		/// 施工工程的名称
		/// </summary>
		private string _ProjectName;
		public string ProjectName
		{
			get { return _ProjectName; }
			set { _ProjectName = value; }
		}

		private Dictionary<Team, DutyOfficer> _TeamInformation;
		public Dictionary<Team, DutyOfficer> TeamInformation
		{
			get { return _TeamInformation; }
			set { _TeamInformation = value; }
		}

		private List<SupervisionDepartment> _SupervisionDepartments;
		public List<SupervisionDepartment> SupervisionDepartments
		{
			get { return _SupervisionDepartments; }
			set { _SupervisionDepartments = value; }
		}

		/// <summary>
		/// 工程所属的企业的信息
		/// </summary>
		private Company _Company;
		public Company Company
		{
			get { return _Company; }
			set { _Company = value; }
		}

		private Region _Region;
		public Region Region
		{
			get { return _Region; }
			set { _Region = value; }
		}

		private DutyOfficer _DutyOfficer;
		public DutyOfficer DutyOfficer
		{
			get { return _DutyOfficer; }
			set { _DutyOfficer = value; }
		}

		/// <summary>
		/// 工程的起始时间
		/// </summary>
		private DateTime _StartOn;
		public DateTime StartOn
		{
			get { return _StartOn; }
			set { _StartOn = value; }
		}

		/// <summary>
		/// 工程计划完工时间
		/// </summary>
		private DateTime _EndOn_Plan;
		public DateTime EndOn_Plan
		{
			get { return _EndOn_Plan; }
			set { _EndOn_Plan = value; }
		}

		/// <summary>
		/// 工程实际完工时间
		/// 如果该字段为MinValue则表示工程没有完工
		/// </summary>
		private DateTime _EndOn_Fact;
		public DateTime EndOn_Fact
		{
			get { return _EndOn_Fact; }
			set { _EndOn_Fact = value; }
		}

		/// <summary>
		/// 工地中心的经度
		/// </summary>
		private double _Location_East;
		public double Location_East
		{
			get { return _Location_East; }
			set { _Location_East = value; }
		}

		/// <summary>
		/// 工地中心的纬度
		/// </summary>
		private double _Location_North;
		public double Location_North
		{
			get { return _Location_North; }
			set { _Location_North = value; }
		}

		private string _Node_Phone;
		public string Node_Phone
		{
			get { return _Node_Phone; }
			set { _Node_Phone = value; }
		}
		#endregion

		public void Save()
		{
			object endOn_Fact;
			if (EndOn_Fact == DateTime.MinValue)
				endOn_Fact = DBNull.Value;
			else
				endOn_Fact = EndOn_Fact;
			if (ID == 0)
			{
				_ID = DBHelper.InsertCommand("Project_Insert", CommandType.StoredProcedure,
					new SqlParameter("@projectname", ProjectName),
					new SqlParameter("@companyid", Company.ID),
					new SqlParameter("@regionid", Region.ID),
					new SqlParameter("@dutyofficer_name", DutyOfficer.PersonName),
					new SqlParameter("@dutyofficer_phonenumber", DutyOfficer.PhoneNumber),
					new SqlParameter("@starton", StartOn.Date),
					new SqlParameter("@endon_plan", EndOn_Plan.Date),
					new SqlParameter("@endon_fact", endOn_Fact),
					new SqlParameter("@location_east", Location_East),
					new SqlParameter("@location_north", Location_North),
					new SqlParameter("@node_phone", Node_Phone));
			}
			else
			{
				DBHelper.UpdateDeleteCommand("Project_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@projectname", ProjectName),
					new SqlParameter("@companyid", Company.ID),
					new SqlParameter("@regionid", Region.ID),
					new SqlParameter("@dutyofficer_name", DutyOfficer.PersonName),
					new SqlParameter("@dutyofficer_phonenumber", DutyOfficer.PhoneNumber),
					new SqlParameter("@starton", StartOn.Date),
					new SqlParameter("@endon_plan", EndOn_Plan.Date),
					new SqlParameter("@endon_fact", endOn_Fact),
					new SqlParameter("@location_east", Location_East),
					new SqlParameter("@location_north", Location_North),
					new SqlParameter("@node_phone", Node_Phone));
				DBHelper.UpdateDeleteCommand("Project-Team_Delete_projectid", CommandType.StoredProcedure,
					new SqlParameter("@projectid", ID));
				DBHelper.UpdateDeleteCommand("Project-Team_Delete_projectid", CommandType.StoredProcedure,
					new SqlParameter("@projectid", ID));
			}
			foreach (Team team in TeamInformation.Keys)
				DBHelper.UpdateDeleteCommand("Project-Team_Insert", CommandType.StoredProcedure,
					new SqlParameter("@projectid", ID),
					new SqlParameter("@teamid", team.ID),
					new SqlParameter("@dutyofficer_name", TeamInformation[team].PersonName),
					new SqlParameter("@dutyofficer_phonenumber", TeamInformation[team].PhoneNumber));
			foreach (SupervisionDepartment supervisionDepartment in SupervisionDepartments)
				DBHelper.UpdateDeleteCommand("Project-SupervisionDepartment_Insert", CommandType.StoredProcedure,
					new SqlParameter("@projectid", ID),
					new SqlParameter("@supervisiondepartmentid", supervisionDepartment.ID));
		}

		private static List<Project> toList(DataTable dt)
		{
			List<Project> result = new List<Project>();
			foreach (DataRow row in dt.Rows)
			{
				Project project = new Project();
				project._ID = Int32.Parse(row["id"].ToString());
				project.ProjectName = row["projectname"].ToString();
				project.Company = Company.Get_ByID(Int32.Parse(row["companyid"].ToString()));
				project.Region = Region.Get_ByID(Int32.Parse(row["regionid"].ToString()));
				project.DutyOfficer.PersonName = row["dutyofficer_name"].ToString();
				project.DutyOfficer.PhoneNumber = row["dutyofficer_phonenumber"].ToString();
				project.StartOn = DateTime.Parse(row["starton"].ToString());
				project.EndOn_Plan = DateTime.Parse(row["endon_plan"].ToString());
				if (row["endon_fact"] == DBNull.Value)
					project.EndOn_Fact = DateTime.MinValue;
				else
					project.EndOn_Fact = DateTime.Parse(row["endon_fact"].ToString());
				project.Location_East = Double.Parse(row["location_east"].ToString());
				project.Location_North = Double.Parse(row["location_north"].ToString());
				project.Node_Phone = row["node_phone"].ToString();

				DataTable dt_teamInformation = DBHelper.SelectCommand("Project-Team_projectid", CommandType.StoredProcedure,
					new SqlParameter("@projectid", project.ID));
				project.TeamInformation = new Dictionary<Team, DutyOfficer>();
				foreach (DataRow row_teamInformation in dt_teamInformation.Rows)
					project.TeamInformation.Add(
						Team.Get_ByID(Int32.Parse(row_teamInformation["teamid"].ToString())),
						new DutyOfficer()
						{
							PersonName = row_teamInformation["dutyofficer_name"].ToString(),
							PhoneNumber = row_teamInformation["dutyofficer_phonenumber"].ToString()
						});

				DataTable dt_supervisionDepartments = DBHelper.SelectCommand("Project-SupervisionDepartment_projectid", CommandType.StoredProcedure,
					new SqlParameter("@projectid", project.ID));
				project.SupervisionDepartments = new List<SupervisionDepartment>();
				foreach (DataRow row_supervisionDepartment in dt_supervisionDepartments.Rows)
					project.SupervisionDepartments.Add(SupervisionDepartment.Get_ByID(Int32.Parse(row_supervisionDepartment["supervisiondepartmentid"].ToString())));

				result.Add(project);
			}
			return result;
		}

		/// <summary>
		/// 通过指定的项目id获取项目信息
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public static Project Get_ByID(int id)
		{
			List<Project> projects = toList(DBHelper.SelectCommand("Project_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (projects.Count == 0)
				return null;
			return projects[0];
		}

		/// <summary>
		/// 获取所有的项目
		/// </summary>
		/// <returns></returns>
		public static List<Project> Get_All()
		{
			return toList(DBHelper.SelectCommand("Project_all", CommandType.StoredProcedure));
		}

		/// <summary>
		/// 通过项目名称模糊查询获取项目
		/// </summary>
		/// <param name="projectName">项目名称</param>
		/// <returns></returns>
		public static List<Project> Get_ByFuzzyProjectName(string projectName)
		{
			return toList(DBHelper.SelectCommand("Project_projectnameFuzzy", CommandType.StoredProcedure,
				new SqlParameter("@projectname", projectName)));
		}

		/// <summary>
		/// 通过项目是否完工查询项目
		/// </summary>
		/// <param name="finish">true表示已经完工, false表示没有完工. 数据库中没有完工的工程的endon_fact字段为null</param>
		/// <returns></returns>
		public static List<Project> Get_ByState(bool finish)
		{
			if (finish)
				return toList(DBHelper.SelectCommand("Project_finish", CommandType.StoredProcedure));
			return toList(DBHelper.SelectCommand("Project_unfinish", CommandType.StoredProcedure));
		}
	}
}
