﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataViewer_Entity
{
	public class TeamLevel : IEntity
	{
		public TeamLevel()
		{
			_ID = 0;
			LevelName = "";
		}

        #region Properties
        /// <summary>
        /// 资质等级ID
        /// </summary>
        private int _ID;
		public int ID
		{
			get { return _ID; }
		}

        /// <summary>
        /// 资质等级名称
        /// </summary>
		private string _LevelName;
		public string LevelName
		{
			get { return _LevelName; }
			set { _LevelName = value; }
		}
        #endregion

		public void Save()
		{
			if (ID == 0)
				_ID = DBHelper.InsertCommand("TeamLevel_Insert", CommandType.StoredProcedure,
					new SqlParameter("@levelname", LevelName));
			else
				DBHelper.UpdateDeleteCommand("TeamLevel_Update", CommandType.StoredProcedure,
					new SqlParameter("@id", ID),
					new SqlParameter("@levelname", LevelName));
		}

        /// <summary>
        /// 获取等级名称
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return LevelName;
		}

		private static List<TeamLevel> toList(DataTable dt)
		{
			List<TeamLevel> result = new List<TeamLevel>();
			foreach (DataRow row in dt.Rows)
			{
				TeamLevel teamLevel = new TeamLevel();
				teamLevel._ID = Int32.Parse(row["id"].ToString());
				teamLevel.LevelName = row["levelname"].ToString();
				result.Add(teamLevel);
			}
			return result;
		}

        /// <summary>
        /// 根据ID获取资质等级
        /// </summary>
        /// <param name="id">要查询的资质等级ID</param>
        /// <returns>返回待查询的资质等级，若没有，返回Null</returns>
		public static TeamLevel Get_ByID(int id)
		{
			List<TeamLevel> teamLevels = toList(DBHelper.SelectCommand("TeamLevel_id", CommandType.StoredProcedure,
				new SqlParameter("@id", id)));
			if (teamLevels.Count == 0)
				return null;
			return teamLevels[0];
		}

        /// <summary>
        /// 获取所有资质等级
        /// </summary>
        /// <returns></returns>
		public static List<TeamLevel> Get_All()
		{
			return toList(DBHelper.SelectCommand("TeamLevel_all", CommandType.StoredProcedure));
		}
	}
}
