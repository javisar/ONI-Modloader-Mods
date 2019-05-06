using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using static KButtonMenu;
using static ReportManager;

namespace ExportDailyReports
{
    [HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
    internal class ExportDailyReports_PauseScreen_OnPrefabInit
    {
        private static FieldInfo buttonsF = AccessTools.Field(typeof(KButtonMenu), "buttons");
        private static void Postfix(PauseScreen __instance)
        {
            Debug.Log(" === ExportDailyReports_PauseScreen_OnPrefabInit Postfix === ");
            /*
            IList<ButtonInfo> buttons = (IList<ButtonInfo>) buttonsF.GetValue(__instance);
            buttons.Insert(
                    buttons.Count-2,
                    new ButtonInfo("Export Daily Reports", Action.NumActions, OnExportDailyReports)
                );
            */
            List<ButtonInfo> ls1 = new List<ButtonInfo>((ButtonInfo[])(IList<ButtonInfo>)buttonsF.GetValue(__instance));
            ls1.Insert(ls1.Count - 2, new ButtonInfo("Export Daily Reports", Action.NumActions, OnExportDailyReports));
            buttonsF.SetValue(__instance, ls1);
        }

        private static void OnExportDailyReports()
        {
            //ConfirmDecision(UI.FRONTEND.MAINMENU.DESKTOPQUITCONFIRM, OnDesktopQuitConfirm);
            List<DailyReport> dailyReports = ReportManager.Instance.reports;

            foreach (var report in dailyReports)
            {
                try
                {
                    Debug.Log(report.day);
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
                            CreateHeader(reportGroup.Value);
                        }
                        else if (flag2)
                        {
                            CreateOrUpdateLine(entry, reportGroup.Value, flag2);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }


        private static void CreateHeader(ReportManager.ReportGroup reportGroup)
        {
            Debug.Log(reportGroup.stringKey);
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

        private static void CreateOrUpdateLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup, bool is_line_active)
        {
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
            SetMainEntry(entry, reportGroup);
        }


        public static void SetMainEntry(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
        {
            SetLine(entry, reportGroup);
            addedValue = float.NegativeInfinity;
            removedValue = float.NegativeInfinity;
            netValue = float.NegativeInfinity;

            int currentContextCount = entry.contextEntries.Count;
            for (int i = 0; i < entry.contextEntries.Count; i++)
            {               
                //SetLine(entry.contextEntries[i], reportGroup);
            }
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

        public static void SetLine(ReportManager.ReportEntry entry, ReportManager.ReportGroup reportGroup)
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
            /*
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
            if (entry.context == null)
            {                
                Debug.Log(reportGroup.stringKey);
            }
            else
            {
                Debug.Log(entry.context);                
            }
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
                Debug.Log("Added: "+text+" = " + entry.Positive);
                addedValue = entry.Positive;
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
                Debug.Log("Removed: " + text2 + " = " + entry.Negative);
                removedValue = entry.Negative;
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
                Debug.Log("Net: " + text3 + " = " + entry.Net);
                netValue = entry.Net;
            }
            pos_notes.Clear();
            neg_notes.Clear();
        }

    }
}
