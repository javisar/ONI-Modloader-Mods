using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExportDailyReports
{
	class ReportData
	{
		public List<string> headersGeneral = new List<string>();
		public Dictionary<int, Dictionary<string, string>> dataGeneral = new Dictionary<int, Dictionary<string, string>>();

		public List<string> headersPower = new List<string>();
		public Dictionary<int, Dictionary<string, string>> dataPower = new Dictionary<int, Dictionary<string, string>>();
	}
}
