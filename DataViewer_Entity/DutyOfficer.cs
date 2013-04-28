using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataViewer_Entity
{
	public class DutyOfficer
	{
		public DutyOfficer()
		{
			PersonName = "";
			PhoneNumber = "";
		}

		private string _PersonName;
		public string PersonName
		{
			get { return _PersonName; }
			set { _PersonName = value; }
		}

		private string _PhoneNumber;
		public string PhoneNumber
		{
			get { return _PhoneNumber; }
			set { _PhoneNumber = value; }
		}
	}
}
