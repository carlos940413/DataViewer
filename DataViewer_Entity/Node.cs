using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	/// <summary>
	/// If the properties changed, you need to change Save() & toList(DataTable dt)
	/// </summary>
	public class Node : IEntity
	{
		private Node()
		{
			_ID = 0;
			HardwareID = 0;
			Description = "";
		}

		public static Node CreateNode(Area area)
		{
			if (area == null || area.ID == 0)
				return null;
			return new Node() { Area = area };
		}

		#region Properties
		private int _ID;
		public int ID
		{
			get { return _ID; }
		}

		/// <summary>
		/// 通过硬件设置的硬件ID
		/// </summary>
		private int _HardwareID;
		public int HardwareID
		{
			get { return _HardwareID; }
			set { _HardwareID = value; }
		}

		/// <summary>
		/// 节点所属区域
		/// </summary>
		private Area _Area;
		public Area Area
		{
			get { return _Area; }
			set { _Area = value; }
		}

		/// <summary>
		/// 节点的描述信息(限制为500个字符)
		/// </summary>
		private string _Description;
		public string Description
		{
			get { return _Description; }
			set { _Description = value; }
		}
		#endregion

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("Node_Insert", CommandType.StoredProcedure,
					new SqlParameter("@hardwareid", HardwareID),
					new SqlParameter("@areaid", Area.ID),
					new SqlParameter("@description", Description));
			else
				DBHelper.UpdateDeleteCommand("Node_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@hardwareid", HardwareID),
					new SqlParameter("@areaid", Area.ID),
					new SqlParameter("@description", Description));
		}

		public void Delete()
		{
			DBHelper.UpdateDeleteCommand("Node_Delete", CommandType.StoredProcedure,
				new SqlParameter("@id", ID));
			_ID = 0;
		}

		private static List<Node> toList(DataTable dt)
		{
			List<Node> result = new List<Node>();
			foreach (DataRow row in dt.Rows)
			{
				Node node = new Node();
				node._ID = Int32.Parse(row["id"].ToString());
				node.HardwareID = Int32.Parse(row["hardwareid"].ToString());
				node.Area = Area.Get_ByID(Int32.Parse(row["areaid"].ToString()));
				node.Description = row["description"].ToString();
				result.Add(node);
			}
			return result;
		}

		/// <summary>
		/// 获取所有的节点
		/// </summary>
		/// <returns></returns>
		public static List<Node> Get_All()
		{
			return toList(DBHelper.SelectCommand("Node_all", CommandType.StoredProcedure));
		}

		/// <summary>
		/// 通过项目id获取属于该项目的节点
		/// </summary>
		/// <param name="projectid">项目id</param>
		/// <returns></returns>
		public static List<Node> Get_ByAreaID(int areaid)
		{
			return toList(DBHelper.SelectCommand("Node_areaid", CommandType.StoredProcedure,
				new SqlParameter("@areaid", areaid)));
		}

		/// <summary>
		/// 通过指定的节点id获取节点
		/// </summary>
		/// <param name="id">节点id</param>
		/// <returns></returns>
		public static Node Get_ByID(int id)
		{
			List<Node> nodes = toList(DBHelper.SelectCommand("Node_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (nodes.Count != 0)
				return nodes[0];
			return new Node();
		}

		/// <summary>
		/// 通过工程id和硬件id获取节点
		/// </summary>
		/// <param name="projectid">工程id</param>
		/// <param name="hardwareid">硬件设备id</param>
		/// <returns></returns>
		public static Node Get_ByAreaID_HardwareID(int areaid, int hardwareid)
		{
			List<Node> nodes = toList(DBHelper.SelectCommand("Node_areaidANDhardwareid", CommandType.StoredProcedure,
				new SqlParameter("@areaid", areaid),
				new SqlParameter("@hardwareid", hardwareid)));
			if (nodes.Count == 0)
				return new Node();
			return nodes[0];
		}
	}
}
