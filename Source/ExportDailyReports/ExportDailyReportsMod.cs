using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

			List<DailyReport> dailyReports = ReportManager.Instance.reports;

			Dictionary<int, Dictionary<string,string>> data = new Dictionary<int, Dictionary<string, string>>();
			List<string> dataH = new List<string>();

			dataH.Add("Cycle");

			foreach (var report in dailyReports)
            {
                try
                {
					//Debug.Log(report.day);
					
					if (!data.ContainsKey(report.day))
						data.Add(report.day, new Dictionary<string, string>());

					if (!data.GetValueSafe(report.day).ContainsKey("Cycle"))
						data.GetValueSafe(report.day).Add("Cycle", report.day.ToString());
					
					
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
                            CreateOrUpdateLine(entry, reportGroup.Value, flag2, data.GetValueSafe(report.day), dataH);
                        }
                    }

				}
                catch (Exception e)
                {
                    Debug.LogError(e);
                }				
            }



			string reportsPath = System.Reflection.Assembly.GetAssembly(typeof(ExportDailyReports_OptionsMenuScreen_OnPrefabInit)).Location;
			string reportsFile = Path.Combine(Path.GetDirectoryName(SaveLoader.GetActiveSaveFilePath()), Path.GetFileNameWithoutExtension(SaveLoader.GetActiveSaveFilePath()) + "_" + dailyReports.Count + ".csv");
			Debug.Log("Exporting Daily Reports: " + reportsFile);

			try
			{
				
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
							dataS.Append("0,");
					}
					dataS.Append("\n");
				}
				File.WriteAllText(reportsFile, dataS.ToString());

				InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);				
				infoDialogScreen.SetHeader("Export Daily Reports").AddPlainText("Daily reports exported.");
				infoDialogScreen.Show();
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				InfoDialogScreen infoDialogScreen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.InfoDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
				infoDialogScreen.SetHeader("Export Daily Reports").AddPlainText("Error exporting daily reports:\n"+e.ToString());
				infoDialogScreen.Show();
			}
        }


        private static void CreateHeader(ReportManager.ReportGroup reportGroup)
        {
            Debug.Log("header: "+reportGroup.stringKey);            
        }

        private static void CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active, Dictionary<string, string> data, List<string> dataH)
        {			
			SetMainEntry(entry, reportGroup, data, dataH);
        }


		private static float addedValue = float.NegativeInfinity;
		private static float removedValue = float.NegativeInfinity;
		private static float netValue = float.NegativeInfinity;

		public static void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, Dictionary<string, string> data, List<string> dataH)
        {
			addedValue = float.NegativeInfinity;
			removedValue = float.NegativeInfinity;
			netValue = float.NegativeInfinity;

			SetLine(entry, reportGroup, data, dataH);           
        }

        public static void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, Dictionary<string, string> data, List<string> dataH)
        {
			
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

			if (!dataH.Contains(CleanHeader(columnName) + " (Added)"))
				dataH.Add(CleanHeader(columnName) + " (Added)");
			if (!dataH.Contains(CleanHeader(columnName) + " (Removed)"))
				dataH.Add(CleanHeader(columnName) + " (Removed)");
			if (!dataH.Contains(CleanHeader(columnName) + " (Net)"))
				dataH.Add(CleanHeader(columnName) + " (Net)");

			if (!data.ContainsKey(CleanHeader(columnName) + " (Added)"))
				data.Add(CleanHeader(columnName) + " (Added)", addedValue.ToString("F2"));

			if (!data.ContainsKey(CleanHeader(columnName) + " (Removed)"))
				data.Add(CleanHeader(columnName) + " (Removed)", removedValue.ToString("F2"));

			if (!data.ContainsKey(CleanHeader(columnName) + " (Net)"))
				data.Add(CleanHeader(columnName) + " (Net)", netValue.ToString("F2"));			
		}


		public static string CleanHeader(string txt)
		{			
			string outT = txt.Replace(":", "");
			int idx = outT.IndexOf("\">");
			if (idx > 0)
				outT = outT.Substring(idx + 2, outT.IndexOf("</link>") - idx - 2) + outT.Substring(outT.IndexOf("</link>")+7);

			return outT.Trim();
		}
    }
}
