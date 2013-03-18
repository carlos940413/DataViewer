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
        public Project()
        {
            ID = 0;
            ProjectName = "";
            Team = null;
            Company = null;
            StartOn = DateTime.MinValue;
            EndOn_Plan = DateTime.MinValue;
            EndOn_Fact = DateTime.MinValue;
            Location_East = 0;
            Location_North = 0;
        }

        #region Properties
        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
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

        /// <summary>
        /// 施工队信息
        /// </summary>
        private Team _Team;
        public Team Team
        {
            get { return _Team; }
            set { _Team = value; }
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
        #endregion

        public void Save()
        {
            object endOn_Fact;
            if (EndOn_Fact == DateTime.MinValue)
                endOn_Fact = DBNull.Value;
            else
                endOn_Fact = EndOn_Fact;
            if (ID == 0)
                ID = DBHelper.InsertCommand("Project_Insert", CommandType.StoredProcedure,
                    new SqlParameter("@projectname", ProjectName),
                    new SqlParameter("@teamid", Team.ID),
                    new SqlParameter("@companyid", Company.ID),
                    new SqlParameter("@starton", StartOn.Date),
                    new SqlParameter("@endon_plan", EndOn_Plan.Date),
                    new SqlParameter("@endon_fact", endOn_Fact),
                    new SqlParameter("@location_east", Location_East),
                    new SqlParameter("@location_north", Location_North));
            else
                DBHelper.UpdateCommand("Project_Update", CommandType.StoredProcedure,
                    new SqlParameter("@id", ID),
                    new SqlParameter("@projectname", ProjectName),
                    new SqlParameter("@teamid", Team.ID),
                    new SqlParameter("@companyid", Company.ID),
                    new SqlParameter("@starton", StartOn.Date),
                    new SqlParameter("@endon_plan", EndOn_Plan.Date),
                    new SqlParameter("@endon_fact", endOn_Fact),
                    new SqlParameter("@location_east", Location_East),
                    new SqlParameter("@location_north", Location_North));
        }

        private static List<Project> toList(DataTable dt)
        {
            List<Project> result = new List<Project>();
            foreach (DataRow row in dt.Rows)
            {
                Project project = new Project();
                project.ID = Int32.Parse(row["id"].ToString());
                project.ProjectName = row["projectname"].ToString();
                project.Team = Team.Get_ByID(Int32.Parse(row["teamid"].ToString()));
                project.Company = Company.Get_ByID(Int32.Parse(row["companyid"].ToString()));
                project.StartOn = DateTime.Parse(row["starton"].ToString());
                project.EndOn_Plan = DateTime.Parse(row["endon_plan"].ToString());
                if (row["endon_fact"] == DBNull.Value)
                    project.EndOn_Fact = DateTime.MinValue;
                else
                    project.EndOn_Fact = DateTime.Parse(row["endon_fact"].ToString());
                project.Location_East = Double.Parse(row["location_east"].ToString());
                project.Location_North = Double.Parse(row["location_north"].ToString());
                result.Add(project);
            }
            return result;
        }

        public static Project Get_ByID(int id)
        {
            List<Project> projects = toList(DBHelper.SelectCommand("Project_id", CommandType.StoredProcedure,
                new SqlParameter("@id", id)));
            if (projects.Count == 0)
                return new Project();
            return projects[0];
        }

        public static List<Project> Get_All()
        {
            return toList(DBHelper.SelectCommand("Project_all", CommandType.StoredProcedure));
        }
    }
}
