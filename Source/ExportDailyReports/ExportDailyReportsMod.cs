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

			//string AssemblyPath = Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
			string reportsPath = System.Reflection.Assembly.GetAssembly(typeof(ExportDailyReports_OptionsMenuScreen_OnPrefabInit)).Location;
			string reportsFile = Path.Combine(Path.GetDirectoryName(SaveLoader.GetActiveSaveFilePath()),Path.GetFileNameWithoutExtension(SaveLoader.GetActiveSaveFilePath()) + "_" + dailyReports.Count + ".csv");

			StringBuilder csvFields = new StringBuilder("");
			StringBuilder csvData = new StringBuilder("");

			csvFields.Append("Cycle");

			int idx = 0;
			foreach (var report in dailyReports)
            {
                try
                {
					//Debug.Log(report.day);
					csvData.Append(report.day);
					/*
                    foreach (var entry in report.reportEntries)
                    {
                        Debug.Log(entry);
                    }
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
                            //AddSpacer(num);
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
                            CreateOrUpdateLine(entry, reportGroup.Value, flag2, idx, csvFields, csvData);
                        }
                    }

					csvData.Append("\n");
				}
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
				idx++;
            }

			try
			{
				Debug.Log("Exporting Daily Reports: "+reportsFile);
				File.WriteAllText(reportsFile, csvFields.ToString()+"\n"+ csvData.ToString());

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
            /*
            GameObject value = null;
            lineItems.TryGetValue(reportGroup.stringKey, out value);
            if (value == null)
            {
                value = Util.KInstantiateUI(lineItemHeader, contentFolder, force_active: true);
                value.name = "LineItemHeader" + lineItems.Count;
                lineItems[reportGroup.stringKey] = value;
            }
            value.SetActive(value: true);
            ReportScreenHeader component = value.GetComponent<ReportScreenHeader>();
            component.SetMainEntry(reportGroup);
            return value;
            */
        }

        private static void CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active, int idx, StringBuilder csvFields, StringBuilder csvData)
        {
			//Debug.Log("line: " + reportGroup.stringKey);
			/*
            GameObject value = null;
            lineItems.TryGetValue(reportGroup.stringKey, out value);
            if (!is_line_active)
            {
                if (value != null && value.activeSelf)
                {
                    value.SetActive(value: false);
                }
            }
            else
            {
                if (value == null)
                {
                    value = Util.KInstantiateUI(lineItem, contentFolder, force_active: true);
                    value.name = "LineItem" + lineItems.Count;
                    lineItems[reportGroup.stringKey] = value;
                }
                value.SetActive(value: true);
                ReportScreenEntry component = value.GetComponent<ReportScreenEntry>();
                component.SetMainEntry(entry, reportGroup);
            }
            return value;
            */
			SetMainEntry(entry, reportGroup, idx, csvFields, csvData);
        }


        public static void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, int idx, StringBuilder csvFields, StringBuilder csvData)
        {
            SetLine(entry, reportGroup, idx, csvFields, csvData);
            addedValue = float.NegativeInfinity;
            removedValue = float.NegativeInfinity;
            netValue = float.NegativeInfinity;
			/*
            int currentContextCount = entry.contextEntries.Count;
            for (int i = 0; i < entry.contextEntries.Count; i++)
            {               
                SetLine(entry.contextEntries[i], reportGroup);
            }
			*/
            /*
            if (mainRow == null)
            {
                mainRow = Util.KInstantiateUI(rowTemplate.gameObject, base.gameObject, force_active: true).GetComponent<ReportScreenEntryRow>();
                MultiToggle toggle = mainRow.toggle;
                toggle.onClick = (System.Action)Delegate.Combine(toggle.onClick, new System.Action(ToggleContext));
                MultiToggle componentInChildren = mainRow.name.GetComponentInChildren<MultiToggle>();
                componentInChildren.onClick = (System.Action)Delegate.Combine(componentInChildren.onClick, new System.Action(ToggleContext));
                MultiToggle componentInChildren2 = mainRow.added.GetComponentInChildren<MultiToggle>();
                componentInChildren2.onClick = (System.Action)Delegate.Combine(componentInChildren2.onClick, new System.Action(ToggleContext));
                MultiToggle componentInChildren3 = mainRow.removed.GetComponentInChildren<MultiToggle>();
                componentInChildren3.onClick = (System.Action)Delegate.Combine(componentInChildren3.onClick, new System.Action(ToggleContext));
                MultiToggle componentInChildren4 = mainRow.net.GetComponentInChildren<MultiToggle>();
                componentInChildren4.onClick = (System.Action)Delegate.Combine(componentInChildren4.onClick, new System.Action(ToggleContext));
            }
            mainRow.SetLine(entry, reportGroup);
            currentContextCount = entry.contextEntries.Count;
            for (int i = 0; i < entry.contextEntries.Count; i++)
            {
                if (i >= contextRows.Count)
                {
                    ReportScreenEntryRow component = Util.KInstantiateUI(rowTemplate.gameObject, base.gameObject).GetComponent<ReportScreenEntryRow>();
                    contextRows.Add(component);
                }
                contextRows[i].SetLine(entry.contextEntries[i], reportGroup);
            }
            UpdateVisibility();
            */
        }

        private static float addedValue = float.NegativeInfinity;
        private static float removedValue = float.NegativeInfinity;
        private static float netValue = float.NegativeInfinity;

        public static void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, int idx, StringBuilder csvFields, StringBuilder csvData)
        {
			//this.entry = entry;
			//this.reportGroup = reportGroup;
			/*
            ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList pos_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
            entry.IterateNotes(delegate (ReportManager.ReportEntry.Note note)
            {
                if (note.value > 0f)
                {
                    pos_notes.Add(note);
                    //Debug.Log("pos "+note.note+" = "+note.value);
                }
            });
            ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.PooledList neg_notes = ListPool<ReportManager.ReportEntry.Note, ReportScreenEntryRow>.Allocate();
            entry.IterateNotes(delegate (ReportManager.ReportEntry.Note note)
            {
                if (note.value < 0f)
                {
                    neg_notes.Add(note);
                    //Debug.Log("neg " + note.note + " = " + note.value);
                }
            });
            */

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

			//LayoutElement component = name.GetComponent<LayoutElement>();
			/*
            if (entry.context == null)
            {                
                Debug.Log(reportGroup.stringKey);
            }
            else
            {
                Debug.Log(entry.context);                
            }
			*/

			string columnName = (entry.context == null ? reportGroup.stringKey : entry.context);


			if (addedValue != entry.Positive)
            {
                string text = reportGroup.formatfn(entry.Positive);
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num6 = 0f;
                    num6 = ((entry.contextEntries.Count <= 0) ? ((float)pos_notes.Count) : ((float)entry.contextEntries.Count));
                    num6 = Mathf.Max(num6, 1f);
                    text = reportGroup.groupFormatfn(entry.Positive, num6);
                }
                //added.text = text;
                addedValue = entry.Positive;
				//Debug.Log("Added: " + text + " = " + addedValue);
				if (idx == 1) csvFields.Append(","+ CleanHeader(columnName) + " (Added)");
				else if (!csvFields.ToString().Contains(CleanHeader(columnName)))
					Debug.Log("Not found: " + CleanHeader(columnName));
				csvData.Append(","+ addedValue);
			}
			if (removedValue != entry.Negative)
            {
                string text2 = reportGroup.formatfn(entry.Negative);
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num7 = 0f;
                    num7 = ((entry.contextEntries.Count <= 0) ? ((float)neg_notes.Count) : ((float)entry.contextEntries.Count));
                    num7 = Mathf.Max(num7, 1f);
                    text2 = reportGroup.groupFormatfn(entry.Negative, num7);
                }
                //removed.text = text2;
                removedValue = entry.Negative;
				//Debug.Log("Removed: " + text2 + " = " + removedValue);
				if (idx == 1) csvFields.Append("," + CleanHeader(columnName) + " (Removed)");
				else if (!csvFields.ToString().Contains(CleanHeader(columnName)))
					Debug.Log("Not found: " + CleanHeader(columnName));
				csvData.Append("," + removedValue);
			}
            if (netValue != entry.Net)
            {
                string text3 = (reportGroup.formatfn != null) ? reportGroup.formatfn(entry.Net) : entry.Net.ToString();
                if (reportGroup.groupFormatfn != null && entry.context == null)
                {
                    float num8 = 0f;
                    num8 = ((entry.contextEntries.Count <= 0) ? ((float)(pos_notes.Count + neg_notes.Count)) : ((float)entry.contextEntries.Count));
                    num8 = Mathf.Max(num8, 1f);
                    text3 = reportGroup.groupFormatfn(entry.Net, num8);
                }
                //net.text = text3;                
                netValue = entry.Net;
				//Debug.Log("Removed: " + text3 + " = " + netValue);
				if (idx == 1) csvFields.Append("," + CleanHeader(columnName) + " (Removed)");
				else if (!csvFields.ToString().Contains(CleanHeader(columnName)))
					Debug.Log("Not found: "+ CleanHeader(columnName));
				csvData.Append("," + netValue);
			}
            pos_notes.Clear();
            neg_notes.Clear();
        }


		public static string CleanHeader(string txt)
		{
			/*
			Regex pattern = new Regex(@"<link="".+ "">(?<txt1>.+)</link>(?<txt2>.+)");
			Match match = pattern.Match(txt);
			string outT = "";
			if (match.Success)
			{
				outT = match.Groups["txt1"].Value+" "+ match.Groups["txt2"].Value;
			}
			else
			{
				outT = txt;
			}
			*/
			string outT = txt.Replace(":", "");
			int idx = outT.IndexOf("\">");
			if (idx > 0)
				outT = outT.Substring(idx + 2, outT.IndexOf("</link>") - idx - 2) + outT.Substring(outT.IndexOf("</link>")+7);

			return outT.Trim();
		}
    }
}
