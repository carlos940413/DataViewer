using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	public class Region : IEntity
	{
		public Region()
		{
			_ID = 0;
			RegionName = "";
		}

		private int _ID;
		public int ID
		{
			get { return _ID; }
		}

		private string _RegionName;
		public string RegionName
		{
			get { return _RegionName; }
			set { _RegionName = value; }
		}

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("Region_Insert", CommandType.StoredProcedure,
					new SqlParameter("@regionname", RegionName));
			else
				DBHelper.UpdateDeleteCommand("Region_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@regionname", RegionName));
		}

		public override string ToString()
		{
			return RegionName;
		}

		private static List<Region> toList(DataTable dt)
		{
			List<Region> result = new List<Region>();
			foreach (DataRow row in dt.Rows)
			{
				Region region = new Region();
				region._ID = Int32.Parse(row["id"].ToString());
				region.RegionName = row["regionname"].ToString();
				result.Add(region);
			}
			return result;
		}

		public static Region Get_ByID(int id)
		{
			List<Region> regions = toList(DBHelper.SelectCommand("Region_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (regions.Count == 0)
				return null;
			return regions[0];
		}

		public static List<Region> Get_All()
		{
			return toList(DBHelper.SelectCommand("Region_all", CommandType.StoredProcedure));
		}
	}
}
