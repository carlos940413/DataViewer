using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
    public class Concentration
    {
        /// <summary>
        /// 向数据库提交采集的数据
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="acquireOn">采集时间</param>
        /// <param name="concentration">粉尘浓度</param>
        public static void SubmitConcentration(Node node, DateTime acquireOn, double concentration)
        {
            if (node.ID == 0 || acquireOn == DateTime.MinValue)
                return;
            Concentration result = new Concentration();
            result._Node = node;
            result._AcquireOn = acquireOn;
            result._Amount = concentration;
            result.Save();
        }

        private Concentration() { }

        #region Properties
        /// <summary>
        /// 采集该数据的节点
        /// </summary>
        private Node _Node;
        public Node Node
        {
            get { return _Node; }
        }

        /// <summary>
        /// 采集时间
        /// </summary>
        private DateTime _AcquireOn;
        public DateTime AcquireOn
        {
            get { return _AcquireOn; }
        }

        /// <summary>
        /// 粉尘浓度
        /// </summary>
        private double _Amount;
        public double Amount
        {
            get { return _Amount; }
        }
        #endregion

        private void Save()
        {
            DBHelper.UpdateCommand("Concentration_Insert", CommandType.StoredProcedure,
                new SqlParameter("@nodeid", Node.ID),
                new SqlParameter("@acquireon", AcquireOn),
                new SqlParameter("@amount", Amount));
        }

        private static List<Concentration> toList(DataTable dt)
        {
            List<Concentration> result = new List<Concentration>();
            foreach (DataRow row in dt.Rows)
            {
                Concentration concentration = new Concentration();
                concentration._Node = Node.Get_ByID(Int32.Parse(row["nodeid"].ToString()));
                concentration._AcquireOn = DateTime.Parse(row["acquireon"].ToString());
                concentration._Amount = Double.Parse(row["amount"].ToString());
                result.Add(concentration);
            }
            return result;
        }

        /// <summary>
        /// 通过项目id获取浓度平均值
        /// </summary>
        /// <param name="projectid">项目id</param>
        /// <returns></returns>
        public static Dictionary<DateTime, double> GetAverage_ByProjectID(int projectid)
        {
            Dictionary<DateTime, double> result = new Dictionary<DateTime, double>();
            DataTable dt = DBHelper.SelectCommand("Concentration_Average_projectid", CommandType.StoredProcedure,
                new SqlParameter("@projectid", projectid));
            foreach (DataRow row in dt.Rows)
            {
                result.Add(DateTime.Parse(row["acquireon"].ToString()),Double.Parse(row["average_amount"].ToString()));
            }
            return result;
        }

        /// <summary>
        /// 通过节点id获取浓度
        /// </summary>
        /// <param name="nodeid">节点的id</param>
        /// <returns></returns>
        public static List<Concentration> Get_ByNodeID(int nodeid)
        {
            return toList(DBHelper.SelectCommand("Concentration_nodeid", CommandType.StoredProcedure,
                new SqlParameter("@nodeid", nodeid)));
        }

        /// <summary>
        /// 根据时间范围和项目id获取数据的时间戳
        /// </summary>
        /// <param name="projectid">项目id</param>
        /// <param name="starttime">起始时间, MinValue表示不对起始时间做限制</param>
        /// <param name="endtime">结束时间, MinValue表示不对结束时间做限制</param>
        /// <returns></returns>
        public static List<DateTime> GetAcquireOn_ByProjectIDANDStartTimeANDEndTime(int projectid, DateTime starttime, DateTime endtime)
        {
            List<DateTime> result = new List<DateTime>();
            DataTable dt;
            if (starttime == DateTime.MinValue && endtime == DateTime.MinValue)
                dt = DBHelper.SelectCommand("Concentration_acquireon_projectid", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid));
            else if (endtime == DateTime.MinValue)
                dt = DBHelper.SelectCommand("Concentration_acquireon_projectidANDstarttime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@starttime", starttime));
            else if (starttime == DateTime.MinValue)
                dt = DBHelper.SelectCommand("Concentration_acquireon_projectidANDendtime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@endtime", endtime));
            else
                dt = DBHelper.SelectCommand("Concentration_acquireon_projectidANDstarttimeANDendtime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@starttime", starttime),
                    new SqlParameter("@endtime", endtime));
            foreach (DataRow row in dt.Rows)
            {
                result.Add(DateTime.Parse(row[0].ToString()));
            }
            return result;
        }

        /// <summary>
        /// 根据项目id和时间范围提取浓度信息
        /// </summary>
        /// <param name="projectid">项目id</param>
        /// <param name="starttime">起始时间, MinValue表示不做限制</param>
        /// <param name="endtime">结束时间, MinValue表示不做限制</param>
        /// <returns></returns>
        public static List<Concentration> Get_ByProjectIDANDStartTimeANDEndTime(int projectid, DateTime starttime, DateTime endtime)
        {
            if (starttime == DateTime.MinValue && endtime == DateTime.MinValue)
                return toList(DBHelper.SelectCommand("Concentration_projectid", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid)));
            else if (endtime == DateTime.MinValue)
                return toList(DBHelper.SelectCommand("Concentration_projectidANDstarttime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@starttime", starttime)));
            else if (starttime == DateTime.MinValue)
                return toList(DBHelper.SelectCommand("Concentration_projectidANDendtime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@endtime", endtime)));
            else
                return toList(DBHelper.SelectCommand("Concentration_projectidANDstarttimeANDendtime", CommandType.StoredProcedure,
                    new SqlParameter("@projectid", projectid),
                    new SqlParameter("@starttime", starttime),
                    new SqlParameter("@endtime", endtime)));
        }
    }
}
