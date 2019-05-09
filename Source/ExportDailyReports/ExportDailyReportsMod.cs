using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using static KButtonMenu;
using static ReportManager;

namespace ExportDailyReports
{
    [HarmonyPatch(typeof(OptionsMenuScreen), "OnPrefabInit")]
    internal class ExportDailyReports_OptionsMenuScreen_OnPrefabInit
	{		
		private static FieldInfo buttonsF = AccessTools.Field(typeof(KButtonMenu), "buttons");

        private static void Postfix(PauseScreen __instance)
        {
            Debug.Log(" === ExportDailyReports_OptionsMenuScreen_OnPrefabInit Postfix === ");           

			var buttonList = new List<ButtonInfo>((IList<ButtonInfo>)buttonsF.GetValue(__instance)) {
				new ButtonInfo("Export Daily Reports", Action.NumActions, OnExportDailyReports)
			};
			buttonsF.SetValue(__instance, buttonList);
		}


        private static void OnExportDailyReports()
        {
			bool error = false;
			List<DailyReport> dailyReports = ReportManager.Instance.reports;

			ReportData data = new ReportData();

			//data.headersGeneral.Add("Cycle");
			//data.headersPower.Add("Cycle");

			foreach (var report in dailyReports)
            {
                try
                {
					//Debug.Log(report.day);

					AddValueCycle("Cycle", report.day, data.dataGeneral, data.headersGeneral);
					AddValueCycle("Cycle", report.day, data.dataPower, data.headersPower);

					/*
					if (!data.ContainsKey(report.day))
						data.Add(report.day, new Dictionary<string, string>());
					if (!data.GetValueSafe(report.day).ContainsKey("Cycle"))
						data.GetValueSafe(report.day).Add("Cycle", report.day.ToString());

					if (!dataP.ContainsKey(report.day))
						dataP.Add(report.day, new Dictionary<string, string>());
					if (!dataP.GetValueSafe(report.day).ContainsKey("Cycle"))
						dataP.GetValueSafe(report.day).Add("Cycle", report.day.ToString());
					*/

					int num = 1;
                    foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> reportGroup in ReportManager.Instance.ReportGroups)
                    {
						ReportManager.ReportEntry entry = report.GetEntry(reportGroup.Key);
                        int num2 = num;
                        ReportManager.ReportGroup value = reportGroup.Value;
                        if (num2 != value.group)
                        {
                            ReportManager.ReportGroup value2 = reportGroup.Value;
                            num = value2.group;
                        }
                        int num3;
                        if (entry.accumulate == 0f)
                        {
                            ReportManager.ReportGroup value3 = reportGroup.Value;
                            num3 = (value3.reportIfZero ? 1 : 0);
                        }
                        else
                        {
                            num3 = 1;
                        }
                        bool flag2 = (byte)num3 != 0;
                        ReportManager.ReportGroup value4 = reportGroup.Value;
                        if (value4.isHeader)
                        {
                            //CreateHeader(reportGroup.Value);
                        }
                        else if (flag2)
                        {
							//CreateOrUpdateLine(entry, reportGroup.Value, flag2, data.GetValueSafe(report.day), dataH, dataP.GetValueSafe(report.day), dataPH);
							CreateOrUpdateLine(entry, reportGroup.Value, flag2, report.day, data);
						}
                    }

				}
                catch (Exception e)
                {
					error = true;
					Debug.LogError(e);
					InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
					infoDialogScreen.SetHeader("Export Daily Reports").AddPlainText("Error exporting daily reports:\n" + e.ToString());
					infoDialogScreen.Show();
				}				
            }

			if (!error)
			{
				data.headersPower.Sort((f1, f2) =>
					{
						if (f1.Equals("Cycle")) return -1;
						return f1.CompareTo(f2);
					}
				);

				WriteData(dailyReports.Count, data);
			}


		}

		private static void WriteData(int day, ReportData data)
		{
			string reportsPath = System.Reflection.Assembly.GetAssembly(typeof(ExportDailyReports_OptionsMenuScreen_OnPrefabInit)).Location;
			string reportsFile = Path.Combine(Path.GetDirectoryName(SaveLoader.GetActiveSaveFilePath()), Path.GetFileNameWithoutExtension(SaveLoader.GetActiveSaveFilePath()) + "_" + day + "_General" + ".csv");
			string reportsPowerFile = Path.Combine(Path.GetDirectoryName(SaveLoader.GetActiveSaveFilePath()), Path.GetFileNameWithoutExtension(SaveLoader.GetActiveSaveFilePath())  + "_" + day + "_Power" + ".csv");

			try
			{
				WriteCSV(reportsFile, data.dataGeneral, data.headersGeneral);

				WriteCSV(reportsPowerFile, data.dataPower, data.headersPower);

				InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
				infoDialogScreen.SetHeader("Export Daily Reports").AddPlainText("Daily reports exported.");
				infoDialogScreen.Show();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
				infoDialogScreen.SetHeader("Export Daily Reports").AddPlainText("Error exporting daily reports:\n" + e.ToString());
				infoDialogScreen.Show();
			}
		}

		private static void WriteCSV(string file, Dictionary<int, Dictionary<string, string>> data, List<string> dataH)
		{
			Debug.Log("Exporting Daily Reports: " + file);
			StringBuilder dataS = new StringBuilder();
			foreach (var item in dataH)
			{
				dataS.Append(item + ",");
			}
			dataS.Append("\n");


			foreach (var item in data)
			{
				foreach (var col in dataH)
				{
					if (item.Value.ContainsKey(col))
						dataS.Append(item.Value.GetValueSafe(col) + ",");
					else
						dataS.Append("0.00,");
				}
				dataS.Append("\n");
			}
			File.WriteAllText(file, dataS.ToString());
		}

        private static void CreateHeader(ReportManager.ReportGroup reportGroup)
        {
            Debug.Log("header: "+reportGroup.stringKey);            
        }

        private static void CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active, int day, ReportData data)
        {			
			SetMainEntry(entry, reportGroup, day, data);
        }


		private static float addedValue = float.NegativeInfinity;
		private static float removedValue = float.NegativeInfinity;
		private static float netValue = float.NegativeInfinity;

		public static void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, int day, ReportData data)
        {
			addedValue = float.NegativeInfinity;
			removedValue = float.NegativeInfinity;
			netValue = float.NegativeInfinity;

			SetLine(entry, reportGroup, day, data);           
        }

        public static void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, int day, ReportData data)
        {
			Debug.Log("cycle: "+day);
			List<ReportManager.ReportEntry.Note> pos_notes = new List<ReportManager.ReportEntry.Note>();
			entry.IterateNotes(delegate (ReportManager.ReportEntry.Note note)
			{
				if (note.value > 0f)
				{
					pos_notes.Add(note);
					//Debug.Log("pos "+note.note+" = "+note.value);
				}
			});

			List<ReportManager.ReportEntry.Note> neg_notes = new List<ReportManager.ReportEntry.Note>();
            entry.IterateNotes(delegate (ReportManager.ReportEntry.Note note)
            {
                if (note.value < 0f)
                {
                    neg_notes.Add(note);
                    //Debug.Log("neg " + note.note + " = " + note.value);
                }
            });			

			string columnName = (entry.context == null ? reportGroup.stringKey : entry.context);
			string textPos = "";
			string textNeg = "";
			string textNet = "";

			if (addedValue != entry.Positive)
            {
                textPos = reportGroup.formatfn(entry.Positive);
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num6 = 0f;
                    num6 = ((entry.contextEntries.Count <= 0) ? ((float)pos_notes.Count) : ((float)entry.contextEntries.Count));
                    num6 = Mathf.Max(num6, 1f);
                    textPos = reportGroup.groupFormatfn(entry.Positive, num6);
                }
				//added.text = textPos;
				addedValue = entry.Positive;
			}
			if (removedValue != entry.Negative)
            {
                textNeg = reportGroup.formatfn(entry.Negative);
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num7 = 0f;
                    num7 = ((entry.contextEntries.Count <= 0) ? ((float)neg_notes.Count) : ((float)entry.contextEntries.Count));
                    num7 = Mathf.Max(num7, 1f);
                    textNeg = reportGroup.groupFormatfn(entry.Negative, num7);
                }
				//removed.text = textNeg;
				removedValue = entry.Negative;
			}
            if (netValue != entry.Net)
            {
                textNet = (reportGroup.formatfn != null) ? reportGroup.formatfn(entry.Net) : entry.Net.ToString();
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num8 = 0f;
                    num8 = ((entry.contextEntries.Count <= 0) ? ((float)(pos_notes.Count + neg_notes.Count)) : ((float)entry.contextEntries.Count));
                    num8 = Mathf.Max(num8, 1f);
                    textNet = reportGroup.groupFormatfn(entry.Net, num8);
                }
				//net.text = textNet;                
				netValue = entry.Net;
			}
            pos_notes.Clear();
            neg_notes.Clear();

			string cleanedColName = CleanHeader(columnName);
			/*
			if (!dataH.Contains(CleanHeader(columnName) + " (Added)"))
				dataH.Add(CleanHeader(columnName) + " (Added)");
			if (!data.ContainsKey(CleanHeader(columnName) + " (Added)"))
				data.Add(CleanHeader(columnName) + " (Added)", addedValue.ToString("F2"));

			if (!dataH.Contains(CleanHeader(columnName) + " (Removed)"))
				dataH.Add(CleanHeader(columnName) + " (Removed)");
			if (!data.ContainsKey(CleanHeader(columnName) + " (Removed)"))
				data.Add(CleanHeader(columnName) + " (Removed)", removedValue.ToString("F2"));

			if (!dataH.Contains(CleanHeader(columnName) + " (Net)"))
				dataH.Add(CleanHeader(columnName) + " (Net)");
			if (!data.ContainsKey(CleanHeader(columnName) + " (Net)"))
				data.Add(CleanHeader(columnName) + " (Net)", netValue.ToString("F2"));
			*/
			AddValue(cleanedColName + " (Added)", addedValue, data.dataGeneral.GetValueSafe(day), data.headersGeneral);
			AddValue(cleanedColName + " (Removed)", removedValue, data.dataGeneral.GetValueSafe(day), data.headersGeneral);
			AddValue(cleanedColName + " (Net)", netValue, data.dataGeneral.GetValueSafe(day), data.headersGeneral);

			if (cleanedColName.Contains("Power Usage"))
			{
				//if (entry.Net > 0f)
				//{
					Debug.Log(cleanedColName + " TOOLTIP POS:");
					string tooltipP = OnNoteTooltip(entry, reportGroup, data.dataPower.GetValueSafe(day), data.headersPower, entry.Positive, reportGroup.positiveTooltip, reportGroup.posNoteOrder, reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => IsPositiveNote(note), reportGroup.groupFormatfn);
					Debug.Log(tooltipP);
				//}
				//else
				//{

					Debug.Log(cleanedColName + " TOOLTIP NEG:");
					string tooltipN = OnNoteTooltip(entry, reportGroup, data.dataPower.GetValueSafe(day), data.headersPower, entry.Negative, reportGroup.negativeTooltip, reportGroup.negNoteOrder, reportGroup.formatfn, (ReportManager.ReportEntry.Note note) => IsNegativeNote(note), reportGroup.groupFormatfn);
					Debug.Log(tooltipN);
				//}
				
			}

		}


		private static string OnNoteTooltip(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, Dictionary<string, string> dataP, List<string> dataPH, float total_accumulation, string tooltip_text, ReportManager.ReportEntry.Order order, ReportManager.FormattingFn format_fn, Func<ReportManager.ReportEntry.Note, bool> is_note_applicable_cb, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			Debug.Log("OnNoteTooltip");
			List <ReportManager.ReportEntry.Note> notes = new List<ReportManager.ReportEntry.Note>();
			notes.Clear();
			entry.IterateNotes(delegate (ReportManager.ReportEntry.Note note)
			{
				if (is_note_applicable_cb(note))
				{
					notes.Add(note);
				}
			});
			//Debug.Log("1");
			string text = string.Empty;
			float num = 0f;
			num = ((entry.contextEntries.Count <= 0) ? ((float)notes.Count) : ((float)entry.contextEntries.Count));
			num = Mathf.Max(num, 1f);
			//Debug.Log("2");
			foreach (ReportManager.ReportEntry.Note item in Sort(notes, reportGroup.posNoteOrder))
			{
				Debug.Log("3 "+ item.ToString());
				ReportManager.ReportEntry.Note current = item;
				string arg = format_fn(current.value);
				Debug.Log(current.value);
				/*
				if (group_format_fn != null)
				{
					arg = group_format_fn(current.value, num);
				}
				*/
				//Debug.Log("5");
				Debug.Log("dataPH: " + dataPH);
				Debug.Log("current.note: "+ current.note);
				string colName = CleanHeader(current.note);
				Debug.Log("colName: " + colName);

				if (current.value > 0f)
				{				
					AddValue(colName + " (Added)", current.value, dataP, dataPH);
					/*
					if (!dataPH.Contains(colName + " (Added)"))
						dataPH.Add(colName + " (Added)");
					if (!dataP.ContainsKey(colName + " (Added)"))
						dataP.Add(colName + " (Added)", current.value.ToString("F2"));
						*/
				}
				else
				{
					AddValue(colName + " (Removed)", current.value, dataP, dataPH);
					/*
					if (!dataPH.Contains(colName + " (Removed)"))
						dataPH.Add(colName + " (Removed)");
					if (!dataP.ContainsKey(colName + " (Removed)"))
						dataP.Add(colName + " (Removed)", current.value.ToString("F2"));
					*/
				}
				text = string.Format(STRINGS.UI.ENDOFDAYREPORT.NOTES.NOTE_ENTRY_LINE_ITEM, text, current.note, arg);
			}
			string arg2 = format_fn(total_accumulation);
			if (group_format_fn != null && entry.context == null)
			{
				arg2 = group_format_fn(total_accumulation, num);
			}
			return string.Format(tooltip_text + "\n" + text, arg2);
		}

		private static bool IsPositiveNote(ReportManager.ReportEntry.Note note)
		{
			if (note.value > 0f)
			{
				return true;
			}
			return false;
		}

		private static bool IsNegativeNote(ReportManager.ReportEntry.Note note)
		{
			if (note.value < 0f)
			{
				return true;
			}
			return false;
		}

		private static  List<ReportManager.ReportEntry.Note> Sort(List<ReportManager.ReportEntry.Note> notes, ReportManager.ReportEntry.Order order)
		{
			switch (order)
			{
				case ReportManager.ReportEntry.Order.Ascending:
					notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => x.value.CompareTo(y.value));
					break;
				case ReportManager.ReportEntry.Order.Descending:
					notes.Sort((ReportManager.ReportEntry.Note x, ReportManager.ReportEntry.Note y) => y.value.CompareTo(x.value));
					break;
			}
			return notes;
		}


		private static void AddValueCycle(string name, int value, Dictionary<int, Dictionary<string, string>> data, List<string> dataH)
		{
			if (!dataH.Contains("Cycle"))
				dataH.Add("Cycle");
			if (!data.ContainsKey(value))
				data.Add(value, new Dictionary<string, string>());
			if (!data.GetValueSafe(value).ContainsKey(name))
			{
				data.GetValueSafe(value).Add(name, value.ToString());
			}
		}

		private static void AddValue(string name, float value, Dictionary<string, string> data, List<string> dataH)
		{
			if (!dataH.Contains(name))
				dataH.Add(name);
			if (!data.ContainsKey(name))
				data.Add(name, value.ToString("F2"));
		}
		

		
		public static string CleanHeader(string txt)
		{
			//string outT = txt.Replace(":", "");
			string outT = txt.Split(':')[0];
			int idx = outT.IndexOf("\">");
			if (idx > 0)
				outT = outT.Substring(idx + 2, outT.IndexOf("</link>") - idx - 2) + outT.Substring(outT.IndexOf("</link>")+7);

			return outT.Trim();
		}
    }
}
